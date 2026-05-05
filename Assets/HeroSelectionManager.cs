using MomosDefense.Audio;
using MomosDefense.Towers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomosDefense.Heroes
{
    public sealed class HeroSelectionManager : MonoBehaviour
    {
        private const string LogPrefix = "[HeroInput]";
        private const string RawLogPrefix = "[HeroRaw]";

        [Header("Heroes")]
        [SerializeField] private MomoHeroController[] heroes;
        [SerializeField] private MomoHeroController startingHero;

        [Header("Input")]
        [SerializeField] private Camera worldCamera;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private bool enableVerboseInputLogs = false;

        public MomoHeroController[] Heroes => heroes;
        public MomoHeroController SelectedHero { get; private set; }

        public readonly struct PointerHit
        {
            public PointerHit(
                MomoHeroController hero,
                bool blocksMovement,
                bool isGround,
                Vector3 point,
                string colliderName,
                int layer)
            {
                Hero = hero;
                BlocksMovement = blocksMovement;
                IsGround = isGround;
                Point = point;
                ColliderName = colliderName;
                Layer = layer;
            }

            public MomoHeroController Hero { get; }
            public bool BlocksMovement { get; }
            public bool IsGround { get; }
            public Vector3 Point { get; }
            public string ColliderName { get; }
            public int Layer { get; }
        }

        private void Awake()
        {
            if (heroes == null || heroes.Length == 0)
            {
                heroes = FindObjectsByType<MomoHeroController>(FindObjectsInactive.Exclude);
            }

            if (worldCamera == null)
            {
                worldCamera = Camera.main;
            }

            if (startingHero == null && heroes != null && heroes.Length > 0)
            {
                startingHero = heroes[0];
            }

            // Force a clean selection state. This prevents multiple heroes from starting selected.
            if (heroes != null)
            {
                foreach (MomoHeroController hero in heroes)
                {
                    hero?.SetSelected(false);
                }
            }

            SelectHero(startingHero);
        }

        private void Update()
        {
            ProcessKeyboardSelection();

            ProcessPointerInput();
        }

        public void SelectHero(MomoHeroController hero)
        {
            if (hero == null)
            {
                return;
            }

            if (SelectedHero == hero)
            {
                LogVerbose($"SelectHero ignored; already selected {hero.HeroName}.");
                return;
            }

            string previousHeroName = SelectedHero != null ? SelectedHero.HeroName : "None";
            SelectedHero = hero;

            LogVerbose($"Selected hero changed: {previousHeroName} -> {SelectedHero.HeroName}.");
            PrototypeAudioDirector.PlaySelection();

            if (heroes == null)
            {
                return;
            }

            foreach (MomoHeroController selectableHero in heroes)
            {
                if (selectableHero != null)
                {
                    selectableHero.SetSelected(selectableHero == SelectedHero);
                }
            }
        }

        public bool SelectHeroAtIndex(int index)
        {
            return TrySelectHeroAtIndex(index);
        }

        private bool TrySelectHeroAtIndex(int index)
        {
            if (heroes == null || index < 0 || index >= heroes.Length)
            {
                return false;
            }

            MomoHeroController previouslySelected = SelectedHero;
            SelectHero(heroes[index]);
            return SelectedHero != null && SelectedHero != previouslySelected;
        }

        private void ProcessKeyboardSelection()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                TrySelectHeroAtIndex(0);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                TrySelectHeroAtIndex(1);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                TrySelectHeroAtIndex(2);
            }
        }

        public void AwardExperienceToAll(int experience)
        {
            if (heroes == null || experience <= 0)
            {
                return;
            }

            foreach (MomoHeroController hero in heroes)
            {
                hero?.GainExperience(experience);
            }
        }

        private void ProcessPointerInput()
        {
            if (!TryGetPointerActivationPosition(out Vector2 screenPosition, out string pointerSource))
            {
                return;
            }

            LogRaw($"Pointer down from {pointerSource} at {screenPosition} on frame {Time.frameCount}, time {Time.unscaledTime:0.000}. Selected hero: {(SelectedHero != null ? SelectedHero.HeroName : "None")}.");

            if (IsPointerOverUi())
            {
                LogRaw("Pointer down ignored because it is over UI.");
                return;
            }

            if (worldCamera == null)
            {
                worldCamera = Camera.main;
            }

            if (worldCamera == null)
            {
                LogRaw("Pointer down ignored because no world camera is available.");
                return;
            }

            Ray ray = worldCamera.ScreenPointToRay(screenPosition);

            if (TryGetBlockingInteraction(ray, out string blockingColliderName))
            {
                LogVerbose($"Pointer input hit blocking interaction {blockingColliderName}; movement command skipped.");
                return;
            }

            if (SelectedHero == null)
            {
                LogRaw("Pointer down did not issue movement because no hero is selected.");
                return;
            }

            if (TryGetMovePoint(ray, SelectedHero.transform.position.y, out Vector3 movePoint))
            {
                LogVerbose($"Move command: {SelectedHero.HeroName} -> {movePoint}.");
                SelectedHero.SetMoveDestination(movePoint);
                return;
            }

            LogRaw("Pointer down did not resolve to any move point.");
        }

        private bool TryGetPointerActivationPosition(out Vector2 screenPosition, out string pointerSource)
        {
            for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
            {
                Touch touch = Input.GetTouch(touchIndex);
                if (touch.phase == TouchPhase.Began)
                {
                    screenPosition = touch.position;
                    pointerSource = $"touch:{touch.fingerId}";
                    return true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                screenPosition = Input.mousePosition;
                pointerSource = "mouse:left-down";
                return true;
            }

            screenPosition = default;
            pointerSource = string.Empty;
            return false;
        }

        public bool HandleResolvedHits(IReadOnlyList<PointerHit> hits, bool logDecisions)
        {
            if (hits == null || hits.Count == 0)
            {
                if (logDecisions)
                {
                    LogVerbose("No actionable hit found for pointer input.");
                }

                return false;
            }

            for (int hitIndex = 0; hitIndex < hits.Count; hitIndex++)
            {
                PointerHit hit = hits[hitIndex];

                if (hit.Hero != null)
                {
                    if (logDecisions)
                    {
                        LogVerbose($"Ignoring world hero hit {hit.Hero.HeroName}; selection is limited to shortcuts and HUD buttons.");
                    }

                    continue;
                }

                if (hit.BlocksMovement)
                {
                    if (logDecisions)
                    {
                        LogVerbose($"Hit tower/build object {hit.ColliderName}; stopping input handling.");
                    }

                    return false;
                }

                if (!hit.IsGround)
                {
                    if (logDecisions)
                    {
                        LogVerbose($"Hit {hit.ColliderName} on layer {hit.Layer}, but it is outside the ground mask.");
                    }

                    continue;
                }

                if (SelectedHero != null)
                {
                    if (logDecisions)
                    {
                        LogVerbose($"Move command: {SelectedHero.HeroName} -> {hit.Point} via collider {hit.ColliderName}.");
                    }

                    SelectedHero.SetMoveDestination(hit.Point);
                    return true;
                }

                if (logDecisions)
                {
                    LogVerbose($"Ground hit at {hit.Point}, but no hero is currently selected.");
                }

                return false;
            }

            if (logDecisions)
            {
                LogVerbose("No actionable hit found for pointer input.");
            }

            return false;
        }

        private bool TryGetBlockingInteraction(Ray ray, out string colliderName)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f) && hit.collider != null)
            {
                if (hit.collider.GetComponentInParent<TowerBuildNode>() != null
                    || hit.collider.GetComponentInParent<TowerAttack>() != null)
                {
                    colliderName = hit.collider.name;
                    return true;
                }
            }

            colliderName = string.Empty;
            return false;
        }

        private bool TryGetMovePoint(Ray ray, float yHeight, out Vector3 movePoint)
        {
            Plane movePlane = new Plane(Vector3.up, new Vector3(0f, yHeight, 0f));
            if (movePlane.Raycast(ray, out float enter))
            {
                movePoint = ray.GetPoint(enter);
                movePoint.y = yHeight;
                return true;
            }

            movePoint = default;
            return false;
        }

        private static bool IsPointerOverUi()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(touchIndex).fingerId))
                {
                    return true;
                }
            }

            return false;
        }

        private void LogVerbose(string message)
        {
            if (!enableVerboseInputLogs)
            {
                return;
            }

            Debug.Log($"{LogPrefix} {message}");
        }

        private void LogRaw(string message)
        {
            if (!enableVerboseInputLogs)
            {
                return;
            }

            Debug.Log($"{RawLogPrefix} {message}");
        }
    }
}

using MomosDefense.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomosDefense.Heroes
{
    public sealed class HeroSelectionManager : MonoBehaviour
    {
        [SerializeField] private PrototypeHeroController[] heroes;
        [SerializeField] private PrototypeHeroController startingHero;

        public PrototypeHeroController[] Heroes => heroes;
        public PrototypeHeroController SelectedHero { get; private set; }

        private void Awake()
        {
            if (heroes == null || heroes.Length == 0)
            {
                heroes = FindObjectsByType<PrototypeHeroController>(FindObjectsInactive.Exclude);
            }

            if (startingHero == null && heroes != null && heroes.Length > 0)
            {
                startingHero = heroes[0];
            }

            SelectHero(startingHero);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectHeroAtIndex(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectHeroAtIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectHeroAtIndex(2);
            }
        }

        private void LateUpdate()
        {
            if (!DidReceiveMoveCommandThisFrame() || IsPointerOverUi())
            {
                return;
            }

            SelectedHero?.TryMoveToPointer();
        }

        public void SelectHero(PrototypeHeroController hero)
        {
            if (hero == null)
            {
                return;
            }

            SelectedHero = hero;
            PrototypeAudioDirector.PlaySelection();

            foreach (PrototypeHeroController selectableHero in heroes)
            {
                if (selectableHero != null)
                {
                    selectableHero.SetSelected(selectableHero == SelectedHero);
                }
            }
        }

        private void SelectHeroAtIndex(int index)
        {
            if (heroes == null || index < 0 || index >= heroes.Length)
            {
                return;
            }

            SelectHero(heroes[index]);
        }

        public void AwardExperienceToAll(int experience)
        {
            if (heroes == null || experience <= 0)
            {
                return;
            }

            foreach (PrototypeHeroController hero in heroes)
            {
                hero?.GainExperience(experience);
            }
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

        private static bool DidReceiveMoveCommandThisFrame()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }

            for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
            {
                if (Input.GetTouch(touchIndex).phase == TouchPhase.Began)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

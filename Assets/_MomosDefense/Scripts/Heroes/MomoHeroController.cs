using MomosDefense.Combat;
using MomosDefense.Enemies;
using MomosDefense.Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomosDefense.Heroes
{
    public sealed class MomoHeroController : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private GameObject selectionIndicator;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 2;
        [SerializeField] private float momoPopRadius = 3f;
        [SerializeField] private int momoPopDamage = 4;
        [SerializeField] private float momoPopCooldown = 8f;
        [SerializeField] private float momoPopSlowDuration = 2.5f;
        [SerializeField] private float momoPopSlowMultiplier = 0.45f;

        private Vector3 destination;
        private float attackTimer;
        private float momoPopCooldownRemaining;
        private bool isSelected = true;

        public float MomoPopCooldownRemaining => momoPopCooldownRemaining;
        public float MomoPopCooldown => momoPopCooldown;
        public bool CanUseMomoPop => momoPopCooldownRemaining <= 0f;
        public bool IsSelected => isSelected;

        private void Awake()
        {
            destination = transform.position;
            worldCamera = worldCamera == null ? Camera.main : worldCamera;
            UpdateSelectionVisual();
        }

        private void Update()
        {
            UpdateCooldowns();
            ReadMoveInput();
            Move();
            AttackNearestEnemy();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryUseMomoPop();
            }
        }

        private void ReadMoveInput()
        {
            if (!isSelected || worldCamera == null || !Input.GetMouseButtonDown(0) || IsPointerOverUi())
            {
                return;
            }

            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                if (hit.collider.TryGetComponent(out TowerBuildNode _))
                {
                    return;
                }

                destination = hit.point;
                destination.y = transform.position.y;
            }
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }

        private void AttackNearestEnemy()
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer > 0f)
            {
                return;
            }

            Health nearest = null;
            float nearestDistance = float.MaxValue;
            Health[] healthTargets = FindObjectsByType<Health>(FindObjectsInactive.Exclude);

            foreach (Health target in healthTargets)
            {
                if (target.gameObject == gameObject || !target.TryGetComponent(out EnemyPathFollower _))
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= attackRange && distance < nearestDistance)
                {
                    nearest = target;
                    nearestDistance = distance;
                }
            }

            if (nearest == null)
            {
                return;
            }

            nearest.TakeDamage(attackDamage);
            attackTimer = 1f / attacksPerSecond;
        }

        public void TryUseMomoPop()
        {
            if (!CanUseMomoPop)
            {
                return;
            }

            EnemyPathFollower[] enemies = FindObjectsByType<EnemyPathFollower>(FindObjectsInactive.Exclude);

            foreach (EnemyPathFollower enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance > momoPopRadius)
                {
                    continue;
                }

                if (enemy.TryGetComponent(out Health health))
                {
                    health.TakeDamage(momoPopDamage);
                }

                enemy.ApplySlow(momoPopSlowDuration, momoPopSlowMultiplier);
            }

            momoPopCooldownRemaining = momoPopCooldown;
        }

        public void SetSelected(bool selected)
        {
            if (isSelected == selected)
            {
                return;
            }

            isSelected = selected;
            UpdateSelectionVisual();
        }

        private void UpdateCooldowns()
        {
            if (momoPopCooldownRemaining > 0f)
            {
                momoPopCooldownRemaining = Mathf.Max(0f, momoPopCooldownRemaining - Time.deltaTime);
            }
        }

        private void UpdateSelectionVisual()
        {
            if (selectionIndicator != null)
            {
                selectionIndicator.SetActive(isSelected);
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
    }
}

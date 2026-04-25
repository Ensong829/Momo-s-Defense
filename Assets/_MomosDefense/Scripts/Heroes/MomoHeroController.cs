using MomosDefense.Combat;
using MomosDefense.Enemies;
using UnityEngine;

namespace MomosDefense.Heroes
{
    public sealed class MomoHeroController : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 2;

        private Vector3 destination;
        private float attackTimer;

        private void Awake()
        {
            destination = transform.position;
            worldCamera = worldCamera == null ? Camera.main : worldCamera;
        }

        private void Update()
        {
            ReadMoveInput();
            Move();
            AttackNearestEnemy();
        }

        private void ReadMoveInput()
        {
            if (worldCamera == null || !Input.GetMouseButtonDown(0))
            {
                return;
            }

            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
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
    }
}

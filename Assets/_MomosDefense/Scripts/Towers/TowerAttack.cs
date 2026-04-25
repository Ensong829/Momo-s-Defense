using MomosDefense.Combat;
using UnityEngine;

namespace MomosDefense.Towers
{
    public sealed class TowerAttack : MonoBehaviour
    {
        [SerializeField] private float attackRange = 4f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 1;

        private float attackTimer;

        private void Update()
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer > 0f)
            {
                return;
            }

            Health target = FindNearestEnemy();
            if (target == null)
            {
                return;
            }

            target.TakeDamage(attackDamage);
            attackTimer = 1f / attacksPerSecond;
        }

        private Health FindNearestEnemy()
        {
            Health nearest = null;
            float nearestDistance = float.MaxValue;
            Health[] healthTargets = FindObjectsByType<Health>(FindObjectsInactive.Exclude);

            foreach (Health target in healthTargets)
            {
                if (!target.CompareTag("Enemy"))
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

            return nearest;
        }
    }
}

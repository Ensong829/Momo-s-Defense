using MomosDefense.Combat;
using MomosDefense.Enemies;
using UnityEngine;

namespace MomosDefense.Towers
{
    public sealed class TowerAttack : MonoBehaviour
    {
        [SerializeField] private float attackRange = 4f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 1;
        [SerializeField] private int upgradeCost = 90;
        [SerializeField] private int maxLevel = 2;
        [SerializeField] private Color upgradedColor = new Color(0.24f, 0.8f, 1f);

        private float attackTimer;
        private int level = 1;

        public int Level => level;
        public int UpgradeCost => upgradeCost;
        public bool CanUpgrade => level < maxLevel;

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
                if (!target.TryGetComponent(out EnemyPathFollower _))
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

        public bool TryUpgrade()
        {
            if (!CanUpgrade)
            {
                return false;
            }

            level++;
            attackDamage += 1;
            attackRange += 0.75f;
            attacksPerSecond += 0.25f;
            transform.localScale *= 1.15f;

            if (TryGetComponent(out Renderer renderer))
            {
                renderer.material.color = upgradedColor;
            }

            return true;
        }
    }
}

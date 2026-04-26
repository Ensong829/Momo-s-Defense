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
        [SerializeField] private Color buffedColor = new Color(0.45f, 0.95f, 0.45f);

        private float attackTimer;
        private int level = 1;
        private float buffTimer;
        private int temporaryDamageBonus;
        private float temporaryAttackSpeedMultiplier = 1f;
        private Renderer cachedRenderer;
        private Color baseColor;
        private TowerBuildNode ownerNode;

        public int Level => level;
        public int UpgradeCost => upgradeCost;
        public bool CanUpgrade => level < maxLevel;

        public void BindToNode(TowerBuildNode buildNode)
        {
            ownerNode = buildNode;
        }

        private void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();

            if (cachedRenderer != null)
            {
                baseColor = cachedRenderer.material.color;
            }
        }

        private void Update()
        {
            UpdateBuff();
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

            target.TakeDamage(attackDamage + temporaryDamageBonus);
            attackTimer = 1f / (attacksPerSecond * temporaryAttackSpeedMultiplier);
        }

        private void OnMouseDown()
        {
            ownerNode?.TryBuildTower();
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
            RefreshVisualState();

            return true;
        }

        public void ApplyTemporaryBoost(float duration, int damageBonus, float attackSpeedMultiplier)
        {
            if (duration <= 0f)
            {
                return;
            }

            buffTimer = Mathf.Max(buffTimer, duration);
            temporaryDamageBonus = Mathf.Max(temporaryDamageBonus, damageBonus);
            temporaryAttackSpeedMultiplier = Mathf.Max(temporaryAttackSpeedMultiplier, attackSpeedMultiplier);
            RefreshVisualState();
        }

        private void UpdateBuff()
        {
            if (buffTimer <= 0f)
            {
                return;
            }

            buffTimer -= Time.deltaTime;

            if (buffTimer > 0f)
            {
                return;
            }

            temporaryDamageBonus = 0;
            temporaryAttackSpeedMultiplier = 1f;
            RefreshVisualState();
        }

        private void RefreshVisualState()
        {
            if (cachedRenderer == null)
            {
                return;
            }

            if (buffTimer > 0f)
            {
                cachedRenderer.material.color = buffedColor;
            }
            else if (level > 1)
            {
                cachedRenderer.material.color = upgradedColor;
            }
            else
            {
                cachedRenderer.material.color = baseColor;
            }
        }
    }
}

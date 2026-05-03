using MomosDefense.Combat;
using MomosDefense.Core;
using MomosDefense.Enemies;
using UnityEngine;

namespace MomosDefense.Towers
{
    public sealed class TowerAttack : MonoBehaviour
    {
        [SerializeField] private TowerDefinition definition;
        [SerializeField] private string towerName = "Tower";
        [SerializeField] private string towerFamilyId = "Star";
        [SerializeField] private float attackRange = 4f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 1;
        [SerializeField] private int upgradeCost = 90;
        [SerializeField] private int maxLevel = 3;
        [SerializeField] private int damagePerUpgrade = 1;
        [SerializeField] private float rangePerUpgrade = 0.75f;
        [SerializeField] private float attackSpeedPerUpgrade = 0.25f;
        [SerializeField] private float scaleMultiplierPerUpgrade = 1.15f;
        [SerializeField] private float slowDurationOnHit;
        [SerializeField] private float slowMultiplierOnHit = 1f;
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

        public TowerDefinition Definition => definition;
        public string TowerName => towerName;
        public string TowerFamilyId => towerFamilyId;
        public int Level => level;
        public int UpgradeCost => upgradeCost * level;
        public bool CanUpgrade => level < maxLevel;

        public void BindToNode(TowerBuildNode buildNode)
        {
            ownerNode = buildNode;
        }

        public void ApplyPersistentRank(int rank, float equipmentAttackSpeedBonus = 0f)
        {
            int bonusRanks = Mathf.Max(0, rank - 1);
            attackDamage += bonusRanks;
            attackRange += bonusRanks * 0.25f;
            attacksPerSecond += bonusRanks * 0.08f + Mathf.Max(0f, equipmentAttackSpeedBonus);
        }

        public void ApplySpecialization(string specialization)
        {
            switch (specialization)
            {
                case "Focus":
                    attackDamage += 2;
                    attackRange += 0.35f;
                    break;
                case "Volley":
                    attacksPerSecond += 0.35f;
                    break;
                case "Deep Freeze":
                    slowDurationOnHit += 0.65f;
                    slowMultiplierOnHit = Mathf.Min(slowMultiplierOnHit, 0.5f);
                    break;
            }
        }

        private void Awake()
        {
            ApplyDefinition();
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

            EnemyPathFollower target = EnemyRegistry.FindNearest(transform.position, attackRange);
            if (target == null || target.Health == null)
            {
                return;
            }

            target.Health.TakeDamage(attackDamage + temporaryDamageBonus);
            if (slowDurationOnHit > 0f)
            {
                target.ApplySlow(slowDurationOnHit, slowMultiplierOnHit);
            }

            attackTimer = 1f / (attacksPerSecond * temporaryAttackSpeedMultiplier);
        }

        private void OnMouseDown()
        {
            ownerNode?.TryBuildTower();
        }

        private void ApplyDefinition()
        {
            if (definition == null)
            {
                return;
            }

            towerName = definition.DisplayName;
            towerFamilyId = definition.TowerFamilyId;
            attackRange = definition.BaseRange;
            attacksPerSecond = definition.AttacksPerSecond;
            attackDamage = definition.BaseDamage;
            upgradeCost = definition.UpgradeCost;
            maxLevel = definition.MaxLevel;
            damagePerUpgrade = definition.DamagePerUpgrade;
            rangePerUpgrade = definition.RangePerUpgrade;
            attackSpeedPerUpgrade = definition.AttackSpeedPerUpgrade;
            scaleMultiplierPerUpgrade = definition.ScaleMultiplierPerUpgrade;
            slowDurationOnHit = definition.SlowDurationOnHit;
            slowMultiplierOnHit = definition.SlowMultiplierOnHit;
        }

        public bool TryUpgrade()
        {
            if (!CanUpgrade)
            {
                return false;
            }

            level++;
            attackDamage += damagePerUpgrade;
            attackRange += rangePerUpgrade;
            attacksPerSecond += attackSpeedPerUpgrade;
            transform.localScale *= scaleMultiplierPerUpgrade;
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

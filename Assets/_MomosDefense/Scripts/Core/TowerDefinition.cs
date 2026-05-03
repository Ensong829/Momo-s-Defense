using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Tower Definition")]
    public sealed class TowerDefinition : ScriptableObject
    {
        [SerializeField] private string towerFamilyId = "Star";
        [SerializeField] private string displayName = "Star Tower";
        [SerializeField] private int buildCost = 60;
        [SerializeField] private int baseDamage = 1;
        [SerializeField] private float baseRange = 4f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int upgradeCost = 80;
        [SerializeField] private int maxLevel = 3;
        [SerializeField] private int damagePerUpgrade = 1;
        [SerializeField] private float rangePerUpgrade = 0.75f;
        [SerializeField] private float attackSpeedPerUpgrade = 0.25f;
        [SerializeField] private float scaleMultiplierPerUpgrade = 1.15f;
        [SerializeField] private float slowDurationOnHit;
        [SerializeField] private float slowMultiplierOnHit = 1f;

        public string TowerFamilyId => towerFamilyId;
        public string DisplayName => displayName;
        public int BuildCost => buildCost;
        public int BaseDamage => baseDamage;
        public float BaseRange => baseRange;
        public float AttacksPerSecond => attacksPerSecond;
        public int UpgradeCost => upgradeCost;
        public int MaxLevel => maxLevel;
        public int DamagePerUpgrade => damagePerUpgrade;
        public float RangePerUpgrade => rangePerUpgrade;
        public float AttackSpeedPerUpgrade => attackSpeedPerUpgrade;
        public float ScaleMultiplierPerUpgrade => scaleMultiplierPerUpgrade;
        public float SlowDurationOnHit => slowDurationOnHit;
        public float SlowMultiplierOnHit => slowMultiplierOnHit;
    }
}

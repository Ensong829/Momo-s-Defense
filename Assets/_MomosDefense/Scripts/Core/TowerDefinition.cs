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

        public string TowerFamilyId => towerFamilyId;
        public string DisplayName => displayName;
        public int BuildCost => buildCost;
        public int BaseDamage => baseDamage;
        public float BaseRange => baseRange;
        public float AttacksPerSecond => attacksPerSecond;
    }
}

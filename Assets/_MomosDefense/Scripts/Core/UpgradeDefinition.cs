using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Upgrade Definition")]
    public sealed class UpgradeDefinition : ScriptableObject
    {
        public enum UpgradeCategory
        {
            HeroSkill = 0,
            TowerFamily = 1
        }

        [SerializeField] private string upgradeId = "MomoSkillRank";
        [SerializeField] private string displayName = "Momo Pop Rank";
        [SerializeField] private string targetId = "Momo";
        [SerializeField] private UpgradeCategory category = UpgradeCategory.HeroSkill;
        [SerializeField] private int maxRank = 5;
        [SerializeField] private int baseCost = 50;
        [SerializeField] private int costStep = 35;

        public string UpgradeId => upgradeId;
        public string DisplayName => displayName;
        public string TargetId => targetId;
        public UpgradeCategory Category => category;
        public int MaxRank => maxRank;
        public int BaseCost => baseCost;
        public int CostStep => costStep;
    }
}

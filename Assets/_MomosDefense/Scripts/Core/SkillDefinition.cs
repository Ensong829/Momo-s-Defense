using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Skill Definition")]
    public sealed class SkillDefinition : ScriptableObject
    {
        public enum SkillBehavior
        {
            DamageSlowArea = 0,
            TowerBuffArea = 1
        }

        [SerializeField] private string skillId = "MomoPop";
        [SerializeField] private string displayName = "Momo Pop";
        [SerializeField] private string ownerHeroId = "Momo";
        [SerializeField] private SkillBehavior behavior = SkillBehavior.DamageSlowArea;
        [SerializeField] private int baseDamage = 4;
        [SerializeField] private float baseRadius = 3f;
        [SerializeField] private float baseCooldown = 8f;
        [SerializeField] private float slowDuration = 2.5f;
        [SerializeField] private float slowMultiplier = 0.45f;
        [SerializeField] private float towerBuffDuration = 5f;
        [SerializeField] private int towerBuffDamageBonus = 1;
        [SerializeField] private float towerBuffAttackSpeedMultiplier = 1.6f;

        public string SkillId => skillId;
        public string DisplayName => displayName;
        public string OwnerHeroId => ownerHeroId;
        public SkillBehavior Behavior => behavior;
        public int BaseDamage => baseDamage;
        public float BaseRadius => baseRadius;
        public float BaseCooldown => baseCooldown;
        public float SlowDuration => slowDuration;
        public float SlowMultiplier => slowMultiplier;
        public float TowerBuffDuration => towerBuffDuration;
        public int TowerBuffDamageBonus => towerBuffDamageBonus;
        public float TowerBuffAttackSpeedMultiplier => towerBuffAttackSpeedMultiplier;
    }
}

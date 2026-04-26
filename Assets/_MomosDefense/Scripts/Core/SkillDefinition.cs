using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Skill Definition")]
    public sealed class SkillDefinition : ScriptableObject
    {
        [SerializeField] private string skillId = "MomoPop";
        [SerializeField] private string displayName = "Momo Pop";
        [SerializeField] private string ownerHeroId = "Momo";
        [SerializeField] private int baseDamage = 4;
        [SerializeField] private float baseRadius = 3f;
        [SerializeField] private float baseCooldown = 8f;

        public string SkillId => skillId;
        public string DisplayName => displayName;
        public string OwnerHeroId => ownerHeroId;
        public int BaseDamage => baseDamage;
        public float BaseRadius => baseRadius;
        public float BaseCooldown => baseCooldown;
    }
}

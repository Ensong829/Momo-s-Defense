using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Hero Definition")]
    public sealed class HeroDefinition : ScriptableObject
    {
        [SerializeField] private string heroId = "Momo";
        [SerializeField] private string displayName = "Momo";
        [SerializeField] private string role = "Control";
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 2;
        [SerializeField] private int experienceToNextLevel = 4;
        [SerializeField] private int attackDamagePerLevel = 1;
        [SerializeField] private int skillDamagePerLevel = 1;
        [SerializeField] private float moveSpeedPerLevel = 0.2f;

        public string HeroId => heroId;
        public string DisplayName => displayName;
        public string Role => role;
        public int StartingLevel => startingLevel;
        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;
        public float AttacksPerSecond => attacksPerSecond;
        public int AttackDamage => attackDamage;
        public int ExperienceToNextLevel => experienceToNextLevel;
        public int AttackDamagePerLevel => attackDamagePerLevel;
        public int SkillDamagePerLevel => skillDamagePerLevel;
        public float MoveSpeedPerLevel => moveSpeedPerLevel;
    }
}

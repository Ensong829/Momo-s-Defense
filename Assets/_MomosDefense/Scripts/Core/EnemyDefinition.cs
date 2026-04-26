using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Enemy Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [SerializeField] private string enemyId = "Basic";
        [SerializeField] private string displayName = "Basic Enemy";
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private int goldReward = 10;
        [SerializeField] private int experienceReward = 1;

        public string EnemyId => enemyId;
        public string DisplayName => displayName;
        public int MaxHealth => maxHealth;
        public float MoveSpeed => moveSpeed;
        public int GoldReward => goldReward;
        public int ExperienceReward => experienceReward;
    }
}

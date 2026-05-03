using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Level Definition")]
    public sealed class LevelDefinition : ScriptableObject
    {
        [SerializeField] private string levelId = "PrototypeLevel01";
        [SerializeField] private string displayName = "Prototype Crossing";
        [SerializeField] private string objectiveText = "Build towers. Start waves. Defend the path.";
        [SerializeField] private WaveDefinition[] waves;
        [SerializeField] private int startingLives = 20;
        [SerializeField] private int startingGold = 120;
        [SerializeField] private float timeBetweenEnemies = 0.7f;
        [SerializeField] private float timeBetweenWaves = 3f;
        [SerializeField] private int victoryCurrencyReward = 35;

        public string LevelId => levelId;
        public string DisplayName => displayName;
        public string ObjectiveText => objectiveText;
        public WaveDefinition[] Waves => waves;
        public int StartingLives => startingLives;
        public int StartingGold => startingGold;
        public float TimeBetweenEnemies => timeBetweenEnemies;
        public float TimeBetweenWaves => timeBetweenWaves;
        public int VictoryCurrencyReward => victoryCurrencyReward;
    }
}

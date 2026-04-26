using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Wave Definition")]
    public sealed class WaveDefinition : ScriptableObject
    {
        [System.Serializable]
        public sealed class SpawnGroup
        {
            public string enemyId = "Basic";
            public int count = 4;
        }

        [SerializeField] private string waveId = "Wave01";
        [SerializeField] private SpawnGroup[] spawnGroups;

        public string WaveId => waveId;
        public SpawnGroup[] SpawnGroups => spawnGroups;
    }
}

using System.Collections;
using MomosDefense.Core;
using MomosDefense.Enemies;
using MomosDefense.Heroes;
using UnityEngine;

namespace MomosDefense.Waves
{
    public sealed class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject toughEnemyPrefab;
        [SerializeField] private GameObject runnerEnemyPrefab;
        [SerializeField] private GameObject armoredEnemyPrefab;
        [SerializeField] private LevelDefinition levelDefinition;
        [SerializeField] private EnemyPrefabEntry[] enemyCatalog;
        [SerializeField] private EnemyPath enemyPath;
        [SerializeField] private GameState gameState;
        [SerializeField] private HeroSelectionManager heroSelection;
        [SerializeField] private int totalWaves = 3;
        [SerializeField] private int enemiesPerWave = 6;
        [SerializeField] private float timeBetweenEnemies = 0.8f;
        [SerializeField] private float timeBetweenWaves = 4f;

        private int aliveEnemies;
        private bool isSpawningWave;

        public LevelDefinition ActiveLevel => levelDefinition;
        public int CurrentWave { get; private set; }
        public int TotalWaves => levelDefinition != null && levelDefinition.Waves != null && levelDefinition.Waves.Length > 0
            ? levelDefinition.Waves.Length
            : totalWaves;
        public bool IsWaveInProgress => isSpawningWave || aliveEnemies > 0;
        public bool CanStartNextWave => !IsComplete && !IsWaveInProgress && (gameState == null || !gameState.IsGameOver);
        public bool IsComplete => CurrentWave >= TotalWaves && !IsWaveInProgress;

        [System.Serializable]
        public sealed class EnemyPrefabEntry
        {
            public string enemyId = "Basic";
            public GameObject prefab;
        }

        private void Awake()
        {
            levelDefinition = BattleSession.ResolveLevel(levelDefinition);
        }

        public void StartNextWave()
        {
            if (!CanStartNextWave)
            {
                return;
            }

            StartCoroutine(SpawnWave(CurrentWave + 1));
        }

        private IEnumerator SpawnWave(int waveNumber)
        {
            isSpawningWave = true;
            CurrentWave = waveNumber;
            WaveDefinition waveDefinition = GetWaveDefinition(waveNumber);

            if (waveDefinition != null && waveDefinition.SpawnGroups != null && waveDefinition.SpawnGroups.Length > 0)
            {
                foreach (WaveDefinition.SpawnGroup spawnGroup in waveDefinition.SpawnGroups)
                {
                    if (spawnGroup == null)
                    {
                        continue;
                    }

                    for (int enemy = 0; enemy < spawnGroup.count; enemy++)
                    {
                        if (gameState != null && gameState.IsGameOver)
                        {
                            break;
                        }

                        SpawnEnemy(spawnGroup.enemyId, waveNumber, enemy);
                        yield return new WaitForSeconds(GetTimeBetweenEnemies());
                    }
                }
            }
            else
            {
                for (int enemy = 0; enemy < enemiesPerWave; enemy++)
                {
                    if (gameState != null && gameState.IsGameOver)
                    {
                        break;
                    }

                    SpawnEnemy(string.Empty, waveNumber, enemy);
                    yield return new WaitForSeconds(GetTimeBetweenEnemies());
                }
            }

            isSpawningWave = false;

            float waveDelay = GetTimeBetweenWaves();
            if (!IsComplete && waveDelay > 0f)
            {
                yield return new WaitForSeconds(waveDelay);
            }
        }

        private void SpawnEnemy(string enemyId, int waveNumber, int enemyIndex)
        {
            if (enemyPrefab == null || enemyPath == null || gameState == null || gameState.IsGameOver)
            {
                return;
            }

            GameObject prefabToSpawn = ChooseEnemyPrefab(enemyId, waveNumber, enemyIndex);
            GameObject enemy = Instantiate(prefabToSpawn);
            aliveEnemies++;
            EnemyPathFollower follower = enemy.GetComponent<EnemyPathFollower>();
            follower?.Initialize(enemyPath, gameState);

            if (enemy.TryGetComponent(out MomosDefense.Combat.Health health))
            {
                int experienceReward = follower != null ? follower.ExperienceReward : 0;
                health.Died.AddListener(_ =>
                {
                    aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
                    heroSelection?.AwardExperienceToAll(experienceReward);
                });
            }

            follower?.ReachedGoal.AddListener(() => aliveEnemies = Mathf.Max(0, aliveEnemies - 1));
        }

        private GameObject ChooseEnemyPrefab(string enemyId, int waveNumber, int enemyIndex)
        {
            if (!string.IsNullOrEmpty(enemyId) && enemyCatalog != null)
            {
                foreach (EnemyPrefabEntry entry in enemyCatalog)
                {
                    if (entry != null && entry.enemyId == enemyId && entry.prefab != null)
                    {
                        return entry.prefab;
                    }
                }
            }

            if (armoredEnemyPrefab != null && waveNumber >= 3 && enemyIndex % 5 == 4)
            {
                return armoredEnemyPrefab;
            }

            if (runnerEnemyPrefab != null && waveNumber >= 2 && enemyIndex % 4 == 1)
            {
                return runnerEnemyPrefab;
            }

            if (toughEnemyPrefab != null && waveNumber >= 2 && enemyIndex % 3 == 2)
            {
                return toughEnemyPrefab;
            }

            return enemyPrefab;
        }

        private WaveDefinition GetWaveDefinition(int waveNumber)
        {
            if (levelDefinition == null || levelDefinition.Waves == null || waveNumber <= 0 || waveNumber > levelDefinition.Waves.Length)
            {
                return null;
            }

            return levelDefinition.Waves[waveNumber - 1];
        }

        private float GetTimeBetweenEnemies()
        {
            return levelDefinition != null ? levelDefinition.TimeBetweenEnemies : timeBetweenEnemies;
        }

        private float GetTimeBetweenWaves()
        {
            return levelDefinition != null ? levelDefinition.TimeBetweenWaves : timeBetweenWaves;
        }
    }
}

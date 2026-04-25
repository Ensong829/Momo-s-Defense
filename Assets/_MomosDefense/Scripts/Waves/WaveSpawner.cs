using System.Collections;
using MomosDefense.Core;
using MomosDefense.Enemies;
using UnityEngine;

namespace MomosDefense.Waves
{
    public sealed class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject toughEnemyPrefab;
        [SerializeField] private EnemyPath enemyPath;
        [SerializeField] private GameState gameState;
        [SerializeField] private int totalWaves = 3;
        [SerializeField] private int enemiesPerWave = 6;
        [SerializeField] private float timeBetweenEnemies = 0.8f;
        [SerializeField] private float timeBetweenWaves = 4f;

        private int aliveEnemies;
        private bool isSpawningWave;

        public int CurrentWave { get; private set; }
        public int TotalWaves => totalWaves;
        public bool IsWaveInProgress => isSpawningWave || aliveEnemies > 0;
        public bool CanStartNextWave => !IsComplete && !IsWaveInProgress && (gameState == null || !gameState.IsGameOver);
        public bool IsComplete => CurrentWave >= totalWaves && !IsWaveInProgress;

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

            for (int enemy = 0; enemy < enemiesPerWave; enemy++)
            {
                if (gameState != null && gameState.IsGameOver)
                {
                    break;
                }

                SpawnEnemy(waveNumber, enemy);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }

            isSpawningWave = false;

            if (!IsComplete && timeBetweenWaves > 0f)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        private void SpawnEnemy(int waveNumber, int enemyIndex)
        {
            if (enemyPrefab == null || enemyPath == null || gameState == null || gameState.IsGameOver)
            {
                return;
            }

            GameObject prefabToSpawn = ChooseEnemyPrefab(waveNumber, enemyIndex);
            GameObject enemy = Instantiate(prefabToSpawn);
            aliveEnemies++;
            enemy.GetComponent<EnemyPathFollower>()?.Initialize(enemyPath, gameState);

            if (enemy.TryGetComponent(out MomosDefense.Combat.Health health))
            {
                health.Died.AddListener(_ => aliveEnemies = Mathf.Max(0, aliveEnemies - 1));
            }

            EnemyPathFollower follower = enemy.GetComponent<EnemyPathFollower>();
            if (follower != null)
            {
                follower.ReachedGoal.AddListener(() => aliveEnemies = Mathf.Max(0, aliveEnemies - 1));
            }
        }

        private GameObject ChooseEnemyPrefab(int waveNumber, int enemyIndex)
        {
            if (toughEnemyPrefab != null && waveNumber >= 2 && enemyIndex % 3 == 2)
            {
                return toughEnemyPrefab;
            }

            return enemyPrefab;
        }
    }
}

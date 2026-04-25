using System.Collections;
using MomosDefense.Core;
using MomosDefense.Enemies;
using UnityEngine;

namespace MomosDefense.Waves
{
    public sealed class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private EnemyPath enemyPath;
        [SerializeField] private GameState gameState;
        [SerializeField] private int totalWaves = 3;
        [SerializeField] private int enemiesPerWave = 6;
        [SerializeField] private float timeBetweenEnemies = 0.8f;
        [SerializeField] private float timeBetweenWaves = 4f;

        private int aliveEnemies;

        public int CurrentWave { get; private set; }
        public int TotalWaves => totalWaves;
        public bool IsSpawningComplete { get; private set; }
        public bool IsComplete => IsSpawningComplete && aliveEnemies == 0;

        private void Start()
        {
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            for (int wave = 0; wave < totalWaves; wave++)
            {
                if (gameState != null && gameState.IsGameOver)
                {
                    break;
                }

                CurrentWave = wave + 1;

                for (int enemy = 0; enemy < enemiesPerWave; enemy++)
                {
                    if (gameState != null && gameState.IsGameOver)
                    {
                        break;
                    }

                    SpawnEnemy();
                    yield return new WaitForSeconds(timeBetweenEnemies);
                }

                yield return new WaitForSeconds(timeBetweenWaves);
            }

            IsSpawningComplete = true;
        }

        private void SpawnEnemy()
        {
            if (enemyPrefab == null || enemyPath == null || gameState == null || gameState.IsGameOver)
            {
                return;
            }

            GameObject enemy = Instantiate(enemyPrefab);
            aliveEnemies++;
            enemy.GetComponent<EnemyPathFollower>()?.Initialize(enemyPath, gameState);

            if (enemy.TryGetComponent(out MomosDefense.Combat.Health health))
            {
                health.Died.AddListener(_ => aliveEnemies--);
            }

            EnemyPathFollower follower = enemy.GetComponent<EnemyPathFollower>();
            if (follower != null)
            {
                follower.ReachedGoal.AddListener(() => aliveEnemies--);
            }
        }
    }
}

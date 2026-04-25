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

        private void Start()
        {
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            for (int wave = 0; wave < totalWaves; wave++)
            {
                for (int enemy = 0; enemy < enemiesPerWave; enemy++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(timeBetweenEnemies);
                }

                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        private void SpawnEnemy()
        {
            if (enemyPrefab == null || enemyPath == null || gameState == null || gameState.IsGameOver)
            {
                return;
            }

            GameObject enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<EnemyPathFollower>()?.Initialize(enemyPath, gameState);
        }
    }
}


using MomosDefense.Core;
using MomosDefense.Combat;
using UnityEngine;

namespace MomosDefense.Enemies
{
    public sealed class EnemyPathFollower : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private int lifeDamage = 1;
        [SerializeField] private int goldReward = 10;

        private EnemyPath path;
        private int waypointIndex;
        private GameState gameState;
        private Health health;
        private bool reachedGoal;

        public int GoldReward => goldReward;

        private void Awake()
        {
            health = GetComponent<Health>();
            if (health != null)
            {
                health.Died.AddListener(HandleDied);
            }
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died.RemoveListener(HandleDied);
            }
        }

        public void Initialize(EnemyPath enemyPath, GameState state)
        {
            path = enemyPath;
            gameState = state;
            waypointIndex = 0;

            if (path != null && path.Waypoints.Count > 0)
            {
                transform.position = path.Waypoints[0].position;
            }
        }

        private void Update()
        {
            if (path == null || path.Waypoints.Count == 0 || gameState == null || gameState.IsGameOver)
            {
                return;
            }

            Transform target = path.Waypoints[waypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                return;
            }

            waypointIndex++;

            if (waypointIndex >= path.Waypoints.Count)
            {
                reachedGoal = true;
                gameState.LoseLife(lifeDamage);
                Destroy(gameObject);
            }
        }

        private void HandleDied(Health defeatedHealth)
        {
            if (!reachedGoal)
            {
                gameState?.AddGold(goldReward);
            }
        }
    }
}

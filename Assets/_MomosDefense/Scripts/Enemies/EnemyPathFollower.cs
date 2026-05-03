using MomosDefense.Core;
using MomosDefense.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace MomosDefense.Enemies
{
    public sealed class EnemyPathFollower : MonoBehaviour
    {
        [SerializeField] private EnemyDefinition definition;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private int lifeDamage = 1;
        [SerializeField] private int goldReward = 10;
        [SerializeField] private int experienceReward = 1;

        private EnemyPath path;
        private int waypointIndex;
        private GameState gameState;
        private Health health;
        private bool reachedGoal;
        private float slowTimer;
        private float slowMultiplier = 1f;

        public EnemyDefinition Definition => definition;
        public Health Health => health;
        public int GoldReward => goldReward;
        public int ExperienceReward => experienceReward;
        public UnityEvent ReachedGoal = new UnityEvent();

        private void Awake()
        {
            health = GetComponent<Health>();
            ApplyDefinition();

            if (health != null)
            {
                health.Died.AddListener(HandleDied);
            }

            EnemyRegistry.Register(this);
        }

        private void OnDestroy()
        {
            EnemyRegistry.Unregister(this);

            if (health != null)
            {
                health.Died.RemoveListener(HandleDied);
            }
        }

        private void ApplyDefinition()
        {
            if (definition == null)
            {
                return;
            }

            moveSpeed = definition.MoveSpeed;
            lifeDamage = definition.LifeDamage;
            goldReward = definition.GoldReward;
            experienceReward = definition.ExperienceReward;
            health?.SetMaxHealth(definition.MaxHealth);
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

            UpdateSlow();

            Transform target = path.Waypoints[waypointIndex];
            float currentMoveSpeed = moveSpeed * slowMultiplier;
            transform.position = Vector3.MoveTowards(transform.position, target.position, currentMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                return;
            }

            waypointIndex++;

            if (waypointIndex >= path.Waypoints.Count)
            {
                reachedGoal = true;
                gameState.LoseLife(lifeDamage);
                ReachedGoal?.Invoke();
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

        public void ApplySlow(float duration, float multiplier)
        {
            if (duration <= 0f)
            {
                return;
            }

            slowTimer = Mathf.Max(slowTimer, duration);
            slowMultiplier = Mathf.Clamp(multiplier, 0.1f, 1f);
        }

        private void UpdateSlow()
        {
            if (slowTimer <= 0f)
            {
                slowMultiplier = 1f;
                return;
            }

            slowTimer -= Time.deltaTime;

            if (slowTimer <= 0f)
            {
                slowMultiplier = 1f;
            }
        }
    }
}

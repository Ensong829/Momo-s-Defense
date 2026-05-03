using System.Collections.Generic;
using UnityEngine;

namespace MomosDefense.Enemies
{
    public static class EnemyRegistry
    {
        private static readonly List<EnemyPathFollower> ActiveEnemiesInternal = new List<EnemyPathFollower>(64);

        public static IReadOnlyList<EnemyPathFollower> ActiveEnemies => ActiveEnemiesInternal;

        public static void Register(EnemyPathFollower enemy)
        {
            if (enemy == null || ActiveEnemiesInternal.Contains(enemy))
            {
                return;
            }

            ActiveEnemiesInternal.Add(enemy);
        }

        public static void Unregister(EnemyPathFollower enemy)
        {
            if (enemy == null)
            {
                return;
            }

            ActiveEnemiesInternal.Remove(enemy);
        }

        public static EnemyPathFollower FindNearest(Vector3 origin, float range)
        {
            EnemyPathFollower nearest = null;
            float nearestDistanceSquared = range * range;

            for (int index = ActiveEnemiesInternal.Count - 1; index >= 0; index--)
            {
                EnemyPathFollower enemy = ActiveEnemiesInternal[index];
                if (enemy == null || !enemy.isActiveAndEnabled || enemy.Health == null || !enemy.Health.IsAlive)
                {
                    ActiveEnemiesInternal.RemoveAt(index);
                    continue;
                }

                float distanceSquared = (enemy.transform.position - origin).sqrMagnitude;
                if (distanceSquared > nearestDistanceSquared)
                {
                    continue;
                }

                nearestDistanceSquared = distanceSquared;
                nearest = enemy;
            }

            return nearest;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MomosDefense.Enemies
{
    public sealed class EnemyPath : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;

        public IReadOnlyList<Transform> Waypoints => waypoints;

        private void OnValidate()
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                waypoints = GetComponentsInChildren<Transform>().Where(child => child != transform).ToArray();
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MomosDefense.Enemies
{
    public sealed class EnemyPath : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float gizmoHeightOffset = 0.15f;
        [SerializeField] private float gizmoRadius = 0.3f;

        public IReadOnlyList<Transform> Waypoints => waypoints;

        private void OnValidate()
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                waypoints = GetComponentsInChildren<Transform>().Where(child => child != transform).ToArray();
            }
        }

        private void OnDrawGizmos()
        {
            DrawWaypoints(false);
        }

        private void OnDrawGizmosSelected()
        {
            DrawWaypoints(true);
        }

        private void DrawWaypoints(bool selected)
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                return;
            }

            Color pathColor = selected ? new Color(1f, 0.92f, 0.3f, 0.95f) : new Color(0.3f, 0.95f, 1f, 0.85f);
            Color pointColor = selected ? new Color(1f, 0.6f, 0.2f, 1f) : new Color(0.15f, 0.85f, 0.35f, 0.95f);

            Vector3? previousPosition = null;

            for (int index = 0; index < waypoints.Length; index++)
            {
                Transform waypoint = waypoints[index];
                if (waypoint == null)
                {
                    continue;
                }

                Vector3 drawPosition = waypoint.position + Vector3.up * gizmoHeightOffset;

                Gizmos.color = pointColor;
                Gizmos.DrawSphere(drawPosition, gizmoRadius);

                Gizmos.color = pathColor;
                if (previousPosition.HasValue)
                {
                    Gizmos.DrawLine(previousPosition.Value, drawPosition);
                }

#if UNITY_EDITOR
                if (selected)
                {
                    Handles.color = Color.white;
                    Handles.Label(drawPosition + Vector3.up * 0.18f, $"WP {index + 1}");
                }
#endif

                previousPosition = drawPosition;
            }
        }
    }
}

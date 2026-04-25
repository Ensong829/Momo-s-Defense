using MomosDefense.Core;
using UnityEngine;

namespace MomosDefense.Towers
{
    public sealed class TowerBuildNode : MonoBehaviour
    {
        [SerializeField] private GameState gameState;
        [SerializeField] private GameObject towerPrefab;
        [SerializeField] private int buildCost = 60;
        [SerializeField] private Vector3 towerOffset = new Vector3(0f, 0.75f, 0f);

        private bool hasTower;
        private Renderer nodeRenderer;
        private Color availableColor;
        private Color occupiedColor = new Color(0.28f, 0.28f, 0.28f);

        private void Awake()
        {
            nodeRenderer = GetComponent<Renderer>();

            if (nodeRenderer != null)
            {
                availableColor = nodeRenderer.material.color;
            }
        }

        private void OnMouseDown()
        {
            TryBuildTower();
        }

        public void TryBuildTower()
        {
            if (hasTower || towerPrefab == null || gameState == null || !gameState.SpendGold(buildCost))
            {
                return;
            }

            Instantiate(towerPrefab, transform.position + towerOffset, Quaternion.identity);
            hasTower = true;
            SetNodeColor(occupiedColor);
        }

        private void SetNodeColor(Color color)
        {
            if (nodeRenderer != null)
            {
                nodeRenderer.material.color = color;
            }
        }

        private void OnMouseEnter()
        {
            if (!hasTower)
            {
                SetNodeColor(Color.white);
            }
        }

        private void OnMouseExit()
        {
            if (!hasTower)
            {
                SetNodeColor(availableColor);
            }
        }
    }
}

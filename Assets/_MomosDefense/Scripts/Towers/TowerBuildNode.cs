using MomosDefense.Core;
using MomosDefense.UI;
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
        private PrototypeHud hud;
        private Color availableColor;
        private Color occupiedColor = new Color(0.28f, 0.28f, 0.28f);

        private void Awake()
        {
            nodeRenderer = GetComponent<Renderer>();

            if (nodeRenderer != null)
            {
                availableColor = nodeRenderer.material.color;
            }

            hud = FindFirstObjectByType<PrototypeHud>();
        }

        private void OnMouseDown()
        {
            TryBuildTower();
        }

        public void TryBuildTower()
        {
            if (hasTower)
            {
                hud?.ShowMessage("This build node is occupied.");
                return;
            }

            if (towerPrefab == null || gameState == null)
            {
                hud?.ShowMessage("Cannot build here yet.");
                return;
            }

            if (!gameState.SpendGold(buildCost))
            {
                hud?.ShowMessage($"Need {buildCost} gold to build.");
                return;
            }

            Instantiate(towerPrefab, transform.position + towerOffset, Quaternion.identity);
            hasTower = true;
            SetNodeColor(occupiedColor);
            hud?.ShowMessage($"Built starter tower (-{buildCost} gold).");
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

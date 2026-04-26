using MomosDefense.Audio;
using MomosDefense.Core;
using MomosDefense.UI;
using UnityEngine;

namespace MomosDefense.Towers
{
    public sealed class TowerBuildNode : MonoBehaviour
    {
        [SerializeField] private GameState gameState;
        [SerializeField] private ProgressionService progressionService;
        [SerializeField] private TowerBuildManager buildManager;
        [SerializeField] private Vector3 towerOffset = new Vector3(0f, 0.75f, 0f);

        private bool hasTower;
        private TowerAttack placedTower;
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
            if (gameState != null && gameState.IsGameOver)
            {
                hud?.ShowMessage("Battle is over.");
                return;
            }

            if (hasTower)
            {
                TryUpgradeTower();
                return;
            }

            TowerBuildManager.TowerBuildOption selectedOption = buildManager != null ? buildManager.SelectedOption : null;

            if (selectedOption == null || selectedOption.towerPrefab == null || gameState == null)
            {
                hud?.ShowMessage("Cannot build here yet.");
                return;
            }

            if (!gameState.SpendGold(selectedOption.buildCost))
            {
                hud?.ShowMessage($"Need {selectedOption.buildCost} gold to build.");
                return;
            }

            GameObject tower = Instantiate(selectedOption.towerPrefab, transform.position + towerOffset, Quaternion.identity);
            placedTower = tower.GetComponent<TowerAttack>();
            placedTower?.BindToNode(this);
            placedTower?.ApplyPersistentRank(
                progressionService != null ? progressionService.GetTowerFamilyRank(selectedOption.displayName) : 1,
                progressionService != null ? progressionService.EquippedTowerAttackSpeedBonus : 0f);
            placedTower?.ApplySpecialization(progressionService != null ? progressionService.GetTowerSpecialization(selectedOption.displayName) : string.Empty);
            hasTower = true;
            SetNodeColor(occupiedColor);
            PrototypeAudioDirector.PlayBuild();
            hud?.ShowMessage($"Built {selectedOption.displayName} (-{selectedOption.buildCost} gold).");
        }

        private void TryUpgradeTower()
        {
            if (placedTower == null)
            {
                hud?.ShowMessage("This build node is occupied.");
                return;
            }

            if (!placedTower.CanUpgrade)
            {
                hud?.ShowMessage("Tower is already upgraded.");
                return;
            }

            int upgradeCost = placedTower.UpgradeCost;
            if (gameState == null || !gameState.SpendGold(upgradeCost))
            {
                hud?.ShowMessage($"Need {upgradeCost} gold to upgrade.");
                return;
            }

            if (placedTower.TryUpgrade())
            {
                PrototypeAudioDirector.PlayUpgrade();
                hud?.ShowMessage($"Tower upgraded (-{upgradeCost} gold).");
            }
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

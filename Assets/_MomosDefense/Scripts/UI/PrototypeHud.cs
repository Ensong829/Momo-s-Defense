using MomosDefense.Core;
using MomosDefense.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace MomosDefense.UI
{
    public sealed class PrototypeHud : MonoBehaviour
    {
        [SerializeField] private GameState gameState;
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private Text livesText;
        [SerializeField] private Text goldText;
        [SerializeField] private Text waveText;
        [SerializeField] private Text resultText;

        private void Update()
        {
            if (gameState == null)
            {
                return;
            }

            livesText.text = $"Lives: {gameState.Lives}";
            goldText.text = $"Gold: {gameState.Gold}";

            if (waveSpawner != null)
            {
                waveText.text = $"Wave: {waveSpawner.CurrentWave}/{waveSpawner.TotalWaves}";
            }

            if (gameState.IsGameOver)
            {
                resultText.text = "Defeat";
            }
            else if (waveSpawner != null && waveSpawner.IsComplete)
            {
                resultText.text = "Victory";
            }
            else
            {
                resultText.text = string.Empty;
            }
        }
    }
}


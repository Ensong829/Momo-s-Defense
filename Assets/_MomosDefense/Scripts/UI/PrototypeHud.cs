using MomosDefense.Core;
using MomosDefense.Heroes;
using MomosDefense.Waves;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MomosDefense.UI
{
    public sealed class PrototypeHud : MonoBehaviour
    {
        [SerializeField] private GameState gameState;
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private MomoHeroController momoHero;
        [SerializeField] private Text livesText;
        [SerializeField] private Text goldText;
        [SerializeField] private Text waveText;
        [SerializeField] private Text momoPopText;
        [SerializeField] private Button momoPopButton;
        [SerializeField] private Text startWaveText;
        [SerializeField] private Button startWaveButton;
        [SerializeField] private Text messageText;
        [SerializeField] private Text resultText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Text restartText;

        private float messageTimer;

        private void Awake()
        {
            if (momoHero == null)
            {
                momoHero = FindFirstObjectByType<MomoHeroController>();
            }

            EnsureMomoPopButton();

            if (momoPopButton != null)
            {
                momoPopButton.onClick.AddListener(UseMomoPop);
            }

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(RestartScene);
                restartButton.gameObject.SetActive(false);
            }

            if (startWaveButton != null)
            {
                startWaveButton.onClick.AddListener(StartNextWave);
            }
        }

        private void OnDestroy()
        {
            if (momoPopButton != null)
            {
                momoPopButton.onClick.RemoveListener(UseMomoPop);
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveListener(RestartScene);
            }

            if (startWaveButton != null)
            {
                startWaveButton.onClick.RemoveListener(StartNextWave);
            }
        }

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

            UpdateMomoPop();
            UpdateStartWave();
            UpdateMessage();

            bool isDefeat = gameState.IsGameOver;
            bool isVictory = waveSpawner != null && waveSpawner.IsComplete;

            if (isDefeat)
            {
                resultText.text = "Defeat";
            }
            else if (isVictory)
            {
                resultText.text = "Victory";
            }
            else
            {
                resultText.text = string.Empty;
            }

            UpdateRestart(isDefeat || isVictory);
        }

        private void UpdateMomoPop()
        {
            if (momoHero == null || momoPopText == null || momoPopButton == null)
            {
                return;
            }

            bool isReady = momoHero.CanUseMomoPop;
            momoPopButton.interactable = isReady;
            momoPopText.text = isReady ? "Momo Pop" : $"Momo Pop {momoHero.MomoPopCooldownRemaining:0.0}s";
        }

        private void UseMomoPop()
        {
            momoHero?.TryUseMomoPop();
        }

        private void StartNextWave()
        {
            waveSpawner?.StartNextWave();
        }

        private void RestartScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            string sceneToReload = string.IsNullOrEmpty(activeScene.path) ? activeScene.name : activeScene.path;
            SceneManager.LoadScene(sceneToReload);
        }

        private void UpdateRestart(bool shouldShow)
        {
            if (restartButton == null)
            {
                return;
            }

            if (restartButton.gameObject.activeSelf != shouldShow)
            {
                restartButton.gameObject.SetActive(shouldShow);
            }

            if (restartText != null)
            {
                restartText.text = "Restart";
            }
        }

        private void UpdateStartWave()
        {
            if (startWaveButton == null || waveSpawner == null)
            {
                return;
            }

            bool canStart = waveSpawner.CanStartNextWave && !gameState.IsGameOver;
            bool shouldShow = !waveSpawner.IsComplete && !gameState.IsGameOver;
            startWaveButton.gameObject.SetActive(shouldShow);
            startWaveButton.interactable = canStart;

            if (startWaveText != null)
            {
                startWaveText.text = waveSpawner.CurrentWave == 0 ? "Start Wave" : "Next Wave";
            }
        }

        public void ShowMessage(string message, float duration = 2f)
        {
            if (messageText == null)
            {
                return;
            }

            messageText.text = message;
            messageTimer = duration;
        }

        private void UpdateMessage()
        {
            if (messageText == null || messageTimer <= 0f)
            {
                return;
            }

            messageTimer -= Time.deltaTime;

            if (messageTimer <= 0f)
            {
                messageText.text = string.Empty;
            }
        }

        private void EnsureMomoPopButton()
        {
            if (momoPopButton != null && momoPopText != null)
            {
                return;
            }

            Canvas canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                return;
            }

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject buttonObject = new GameObject("Momo Pop Button");
            buttonObject.transform.SetParent(canvas.transform);

            Image image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.94f, 0.58f, 0.74f, 0.9f);
            momoPopButton = buttonObject.AddComponent<Button>();

            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0f, 0f);
            buttonRect.anchorMax = new Vector2(0f, 0f);
            buttonRect.pivot = new Vector2(0f, 0f);
            buttonRect.anchoredPosition = new Vector2(16f, 16f);
            buttonRect.sizeDelta = new Vector2(190f, 54f);

            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(buttonObject.transform);
            momoPopText = labelObject.AddComponent<Text>();
            momoPopText.text = "Momo Pop";
            momoPopText.font = font;
            momoPopText.fontSize = 22;
            momoPopText.color = Color.white;
            momoPopText.alignment = TextAnchor.MiddleCenter;

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
        }

        public void SetRestartControls(Button button, Text label)
        {
            restartButton = button;
            restartText = label;
        }
    }
}

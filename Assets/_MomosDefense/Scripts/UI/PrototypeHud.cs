using MomosDefense.Core;
using MomosDefense.Heroes;
using MomosDefense.Waves;
using UnityEngine;
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
        [SerializeField] private Text resultText;

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
        }

        private void OnDestroy()
        {
            if (momoPopButton != null)
            {
                momoPopButton.onClick.RemoveListener(UseMomoPop);
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
    }
}

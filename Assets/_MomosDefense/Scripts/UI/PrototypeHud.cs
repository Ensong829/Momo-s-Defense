using MomosDefense.Audio;
using MomosDefense.Core;
using MomosDefense.Heroes;
using MomosDefense.Towers;
using MomosDefense.Waves;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MomosDefense.UI
{
    public sealed class PrototypeHud : MonoBehaviour
    {
        [SerializeField] private GameState gameState;
        [SerializeField] private ProgressionService progressionService;
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private HeroSelectionManager heroSelection;
        [SerializeField] private TowerBuildManager buildManager;
        [SerializeField] private PrototypeHeroController momoHero;
        [SerializeField] private PrototypeHeroController bulwarkHero;
        [SerializeField] private PrototypeHeroController sproutHero;
        [SerializeField] private Text livesText;
        [SerializeField] private Text goldText;
        [SerializeField] private Text waveText;
        [SerializeField] private Text softCurrencyText;
        [SerializeField] private Text skillText;
        [SerializeField] private Button skillButton;
        [SerializeField] private Text momoPortraitText;
        [SerializeField] private Button momoPortraitButton;
        [SerializeField] private Image momoPortraitImage;
        [SerializeField] private Text bulwarkPortraitText;
        [SerializeField] private Button bulwarkPortraitButton;
        [SerializeField] private Image bulwarkPortraitImage;
        [SerializeField] private Text sproutPortraitText;
        [SerializeField] private Button sproutPortraitButton;
        [SerializeField] private Image sproutPortraitImage;
        [SerializeField] private Text towerBuildText;
        [SerializeField] private Button towerBuildButton;
        [SerializeField] private Text burstBuildText;
        [SerializeField] private Button burstBuildButton;
        [SerializeField] private Text frostBuildText;
        [SerializeField] private Button frostBuildButton;
        [SerializeField] private Text startWaveText;
        [SerializeField] private Button startWaveButton;
        [SerializeField] private GameObject progressionPanel;
        [SerializeField] private Text progressionTitleText;
        [SerializeField] private Text progressionSummaryText;
        [SerializeField] private Text progressionToggleText;
        [SerializeField] private Button progressionToggleButton;
        [SerializeField] private Text momoSkillUpgradeText;
        [SerializeField] private Button momoSkillUpgradeButton;
        [SerializeField] private Text bulwarkSkillUpgradeText;
        [SerializeField] private Button bulwarkSkillUpgradeButton;
        [SerializeField] private Text sproutSkillUpgradeText;
        [SerializeField] private Button sproutSkillUpgradeButton;
        [SerializeField] private Text starTowerUpgradeText;
        [SerializeField] private Button starTowerUpgradeButton;
        [SerializeField] private Text burstTowerUpgradeText;
        [SerializeField] private Button burstTowerUpgradeButton;
        [SerializeField] private Text frostTowerUpgradeText;
        [SerializeField] private Button frostTowerUpgradeButton;
        [SerializeField] private Text starSpecializationText;
        [SerializeField] private Button starSpecializationButton;
        [SerializeField] private Text burstSpecializationText;
        [SerializeField] private Button burstSpecializationButton;
        [SerializeField] private Text frostSpecializationText;
        [SerializeField] private Button frostSpecializationButton;
        [SerializeField] private Text resetProgressionText;
        [SerializeField] private Button resetProgressionButton;
        [SerializeField] private Text rewardText;
        [SerializeField] private Text messageText;
        [SerializeField] private Text objectiveText;
        [SerializeField] private Text resultText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Text restartText;
        [SerializeField] private string fallbackShellSceneName = "LevelSelect";
        [SerializeField] private bool enableKeyboardShellShortcuts = true;
        [SerializeField] private KeyCode restartFromShellKey = KeyCode.R;
        [SerializeField] private KeyCode exitToShellKey = KeyCode.Escape;
        [SerializeField] private bool autoReturnToShellOnBattleEnd = true;
        [SerializeField] private float autoReturnDelaySeconds = 2f;

        private readonly Color idlePortraitColor = new Color(0.42f, 0.36f, 0.42f, 0.9f);
        private readonly Color momoSelectedColor = new Color(1f, 0.7f, 0.88f, 0.95f);
        private readonly Color bulwarkSelectedColor = new Color(0.95f, 0.8f, 0.45f, 0.95f);
        private readonly Color sproutSelectedColor = new Color(0.58f, 0.92f, 0.58f, 0.95f);
        private float messageTimer;
        private bool playedVictoryAudio;
        private bool playedDefeatAudio;
        private bool grantedVictoryReward;
        private bool queuedAutoReturnToShell;
        private float autoReturnTimer;

        private void Awake()
        {
            if (heroSelection == null)
            {
                heroSelection = FindFirstObjectByType<HeroSelectionManager>();
            }

            if (buildManager == null)
            {
                buildManager = FindFirstObjectByType<TowerBuildManager>();
            }

            if (progressionService == null)
            {
                progressionService = FindFirstObjectByType<ProgressionService>();
            }

            ApplyPersistentProgression();

            EnsureSkillButton();

            if (skillButton != null)
            {
                skillButton.onClick.AddListener(UseSelectedSkill);
            }

            if (momoPortraitButton != null)
            {
                momoPortraitButton.onClick.AddListener(SelectMomo);
            }

            if (bulwarkPortraitButton != null)
            {
                bulwarkPortraitButton.onClick.AddListener(SelectBulwark);
            }

            if (sproutPortraitButton != null)
            {
                sproutPortraitButton.onClick.AddListener(SelectSprout);
            }

            if (towerBuildButton != null)
            {
                towerBuildButton.onClick.AddListener(() => SelectTowerOption(0));
            }

            if (burstBuildButton != null)
            {
                burstBuildButton.onClick.AddListener(() => SelectTowerOption(1));
            }

            if (frostBuildButton != null)
            {
                frostBuildButton.onClick.AddListener(() => SelectTowerOption(2));
            }

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(RestartFromShell);
                restartButton.gameObject.SetActive(false);
            }

            if (momoSkillUpgradeButton != null)
            {
                momoSkillUpgradeButton.onClick.AddListener(() => UpgradeHeroSkill("Momo"));
            }

            if (bulwarkSkillUpgradeButton != null)
            {
                bulwarkSkillUpgradeButton.onClick.AddListener(() => UpgradeHeroSkill("Bulwark"));
            }

            if (sproutSkillUpgradeButton != null)
            {
                sproutSkillUpgradeButton.onClick.AddListener(() => UpgradeHeroSkill("Sprout"));
            }

            if (starTowerUpgradeButton != null)
            {
                starTowerUpgradeButton.onClick.AddListener(() => UpgradeTowerFamily("Star"));
            }

            if (burstTowerUpgradeButton != null)
            {
                burstTowerUpgradeButton.onClick.AddListener(() => UpgradeTowerFamily("Burst"));
            }

            if (frostTowerUpgradeButton != null)
            {
                frostTowerUpgradeButton.onClick.AddListener(() => UpgradeTowerFamily("Frost"));
            }

            if (starSpecializationButton != null)
            {
                starSpecializationButton.onClick.AddListener(() => ChooseTowerSpecialization("Star"));
            }

            if (burstSpecializationButton != null)
            {
                burstSpecializationButton.onClick.AddListener(() => ChooseTowerSpecialization("Burst"));
            }

            if (frostSpecializationButton != null)
            {
                frostSpecializationButton.onClick.AddListener(() => ChooseTowerSpecialization("Frost"));
            }

            if (resetProgressionButton != null)
            {
                resetProgressionButton.onClick.AddListener(ResetProgression);
            }

            if (progressionToggleButton != null)
            {
                progressionToggleButton.onClick.AddListener(ToggleProgressionPanel);
            }

            if (startWaveButton != null)
            {
                startWaveButton.onClick.AddListener(StartNextWave);
            }
        }

        private void OnDestroy()
        {
            if (skillButton != null)
            {
                skillButton.onClick.RemoveListener(UseSelectedSkill);
            }

            if (momoPortraitButton != null)
            {
                momoPortraitButton.onClick.RemoveListener(SelectMomo);
            }

            if (bulwarkPortraitButton != null)
            {
                bulwarkPortraitButton.onClick.RemoveListener(SelectBulwark);
            }

            if (sproutPortraitButton != null)
            {
                sproutPortraitButton.onClick.RemoveListener(SelectSprout);
            }

            if (towerBuildButton != null)
            {
                towerBuildButton.onClick.RemoveAllListeners();
            }

            if (burstBuildButton != null)
            {
                burstBuildButton.onClick.RemoveAllListeners();
            }

            if (frostBuildButton != null)
            {
                frostBuildButton.onClick.RemoveAllListeners();
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveListener(RestartFromShell);
            }

            if (momoSkillUpgradeButton != null)
            {
                momoSkillUpgradeButton.onClick.RemoveAllListeners();
            }

            if (bulwarkSkillUpgradeButton != null)
            {
                bulwarkSkillUpgradeButton.onClick.RemoveAllListeners();
            }

            if (sproutSkillUpgradeButton != null)
            {
                sproutSkillUpgradeButton.onClick.RemoveAllListeners();
            }

            if (starTowerUpgradeButton != null)
            {
                starTowerUpgradeButton.onClick.RemoveAllListeners();
            }

            if (burstTowerUpgradeButton != null)
            {
                burstTowerUpgradeButton.onClick.RemoveAllListeners();
            }

            if (frostTowerUpgradeButton != null)
            {
                frostTowerUpgradeButton.onClick.RemoveAllListeners();
            }

            if (starSpecializationButton != null)
            {
                starSpecializationButton.onClick.RemoveAllListeners();
            }

            if (burstSpecializationButton != null)
            {
                burstSpecializationButton.onClick.RemoveAllListeners();
            }

            if (frostSpecializationButton != null)
            {
                frostSpecializationButton.onClick.RemoveAllListeners();
            }

            if (resetProgressionButton != null)
            {
                resetProgressionButton.onClick.RemoveListener(ResetProgression);
            }

            if (progressionToggleButton != null)
            {
                progressionToggleButton.onClick.RemoveListener(ToggleProgressionPanel);
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
            UpdateProgressionText();

            if (waveSpawner != null)
            {
                waveText.text = $"Wave: {waveSpawner.CurrentWave}/{waveSpawner.TotalWaves}";
            }

            UpdateSkillButton();
            UpdatePortraits();
            UpdateTowerButtons();
            UpdateStartWave();
            UpdateMessage();
            UpdateObjective();

            bool isDefeat = gameState.IsGameOver;
            bool isVictory = waveSpawner != null && waveSpawner.IsComplete;

            if (isDefeat)
            {
                resultText.text = "Defeat";
                if (!playedDefeatAudio)
                {
                    PrototypeAudioDirector.PlayDefeat();
                    playedDefeatAudio = true;
                }
            }
            else if (isVictory)
            {
                resultText.text = "Victory";
                if (!playedVictoryAudio)
                {
                    GrantVictoryReward();
                    PrototypeAudioDirector.PlayVictory();
                    playedVictoryAudio = true;
                }
            }
            else
            {
                resultText.text = string.Empty;
            }

            UpdateRestart(isDefeat || isVictory);
            UpdateAutoReturn(isDefeat || isVictory);

            if (enableKeyboardShellShortcuts)
            {
                if (Input.GetKeyDown(exitToShellKey))
                {
                    ExitToShell();
                }
                else if (Input.GetKeyDown(restartFromShellKey))
                {
                    RestartFromShell();
                }
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

        private void UseSelectedSkill()
        {
            heroSelection?.SelectedHero?.TryUseSkill();
        }

        private void UpgradeHeroSkill(string heroId)
        {
            if (progressionService == null)
            {
                ShowMessage("Progression is not ready.");
                return;
            }

            if (!progressionService.TryUpgradeHeroSkill(heroId))
            {
                ShowMessage($"Need more crystals for {heroId} skill.");
                return;
            }

            ApplyPersistentProgression();
            PrototypeAudioDirector.PlayUpgrade();
            ShowMessage($"{heroId} skill rank {progressionService.GetHeroSkillRank(heroId)} unlocked.");
        }

        private void UpgradeTowerFamily(string familyId)
        {
            if (progressionService == null)
            {
                ShowMessage("Progression is not ready.");
                return;
            }

            if (!progressionService.TryUpgradeTowerFamily(familyId))
            {
                ShowMessage($"Need more crystals for {familyId} towers.");
                return;
            }

            PrototypeAudioDirector.PlayUpgrade();
            ShowMessage($"{familyId} tower rank {progressionService.GetTowerFamilyRank(familyId)} unlocked.");
        }

        private void ResetProgression()
        {
            progressionService?.ResetPrototypeProgression();
            ShowMessage("Prototype progression reset.");
            RestartFromShell();
        }

        private void ChooseTowerSpecialization(string familyId)
        {
            if (progressionService == null || !progressionService.TryChooseTowerSpecialization(familyId))
            {
                ShowMessage($"{familyId} needs rank 3 first.");
                return;
            }

            PrototypeAudioDirector.PlayUpgrade();
            ShowMessage($"{familyId}: {progressionService.GetTowerSpecialization(familyId)} chosen.");
        }

        private void ToggleProgressionPanel()
        {
            if (progressionPanel != null)
            {
                progressionPanel.SetActive(!progressionPanel.activeSelf);
            }
        }

        private void SelectMomo()
        {
            if (heroSelection != null && momoHero != null)
            {
                heroSelection.SelectHero(momoHero);
            }
        }

        private void SelectBulwark()
        {
            if (heroSelection != null && bulwarkHero != null)
            {
                heroSelection.SelectHero(bulwarkHero);
            }
        }

        private void SelectSprout()
        {
            if (heroSelection != null && sproutHero != null)
            {
                heroSelection.SelectHero(sproutHero);
            }
        }

        private void SelectTowerOption(int optionIndex)
        {
            buildManager?.SelectOption(optionIndex);
        }

        private void UpdateSkillButton()
        {
            if (skillText == null || skillButton == null)
            {
                return;
            }

            PrototypeHeroController selectedHero = heroSelection != null ? heroSelection.SelectedHero : null;

            if (selectedHero == null)
            {
                skillButton.interactable = false;
                skillText.text = "No Skill";
                return;
            }

            skillButton.interactable = selectedHero.CanUseSkill;
            string rankText = progressionService != null
                ? $" R{progressionService.GetHeroSkillRank(selectedHero.HeroName)}"
                : string.Empty;
            skillText.text = selectedHero.CanUseSkill
                ? $"{selectedHero.SkillName}{rankText}"
                : $"{selectedHero.SkillName}{rankText} {selectedHero.SkillCooldownRemaining:0.0}s";
        }

        private void UpdateProgressionText()
        {
            if (progressionService == null)
            {
                return;
            }

            if (softCurrencyText != null)
            {
                softCurrencyText.text = $"Crystals: {progressionService.SoftCurrency}";
            }

            if (momoSkillUpgradeText != null)
            {
                UpdateHeroUpgradeButton("Momo", momoSkillUpgradeText, momoSkillUpgradeButton);
            }

            UpdateHeroUpgradeButton("Bulwark", bulwarkSkillUpgradeText, bulwarkSkillUpgradeButton);
            UpdateHeroUpgradeButton("Sprout", sproutSkillUpgradeText, sproutSkillUpgradeButton);
            UpdateTowerUpgradeButton("Star", starTowerUpgradeText, starTowerUpgradeButton);
            UpdateTowerUpgradeButton("Burst", burstTowerUpgradeText, burstTowerUpgradeButton);
            UpdateTowerUpgradeButton("Frost", frostTowerUpgradeText, frostTowerUpgradeButton);
            UpdateTowerSpecializationButton("Star", starSpecializationText, starSpecializationButton);
            UpdateTowerSpecializationButton("Burst", burstSpecializationText, burstSpecializationButton);
            UpdateTowerSpecializationButton("Frost", frostSpecializationText, frostSpecializationButton);

            if (progressionTitleText != null)
            {
                progressionTitleText.text = "Upgrades";
            }

            if (progressionSummaryText != null)
            {
                progressionSummaryText.text = $"Crystals {progressionService.SoftCurrency}  Charm +{progressionService.EquippedHeroSkillDamageBonus}";
            }

            if (progressionToggleText != null)
            {
                progressionToggleText.text = "Upgrades";
            }

            if (resetProgressionText != null)
            {
                resetProgressionText.text = "Reset";
            }
        }

        private void UpdateHeroUpgradeButton(string heroId, Text label, Button button)
        {
            if (label == null || button == null || progressionService == null)
            {
                return;
            }

            int rank = progressionService.GetHeroSkillRank(heroId);
            bool maxed = rank >= progressionService.MaxSkillRank;
            bool affordable = progressionService.CanUpgradeHeroSkill(heroId);
            label.text = maxed
                ? $"{heroId} Skill Max"
                : $"{heroId} Skill R{rank + 1} {progressionService.GetHeroSkillUpgradeCost(heroId)}c";
            button.interactable = affordable;
            SetButtonStateColor(button, maxed, affordable);
        }

        private void UpdateTowerUpgradeButton(string familyId, Text label, Button button)
        {
            if (label == null || button == null || progressionService == null)
            {
                return;
            }

            int rank = progressionService.GetTowerFamilyRank(familyId);
            bool maxed = rank >= progressionService.MaxSkillRank;
            bool affordable = progressionService.CanUpgradeTowerFamily(familyId);
            label.text = maxed
                ? $"{familyId} Tower Max"
                : $"{familyId} Tower R{rank + 1} {progressionService.GetTowerFamilyUpgradeCost(familyId)}c";
            button.interactable = affordable;
            SetButtonStateColor(button, maxed, affordable);
        }

        private void UpdateTowerSpecializationButton(string familyId, Text label, Button button)
        {
            if (label == null || button == null || progressionService == null)
            {
                return;
            }

            string specialization = progressionService.GetTowerSpecialization(familyId);
            bool chosen = !string.IsNullOrEmpty(specialization);
            bool available = progressionService.CanChooseTowerSpecialization(familyId);
            label.text = chosen ? specialization : $"{familyId} Spec";
            button.interactable = available;
            SetButtonStateColor(button, chosen, available);
        }

        private static void SetButtonStateColor(Button button, bool maxed, bool affordable)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = maxed
                ? new Color(0.45f, 0.7f, 0.5f, 1f)
                : affordable ? new Color(0.95f, 0.82f, 0.4f, 1f) : new Color(0.72f, 0.72f, 0.72f, 1f);
            colors.highlightedColor = affordable ? new Color(1f, 0.9f, 0.55f, 1f) : colors.normalColor;
            button.colors = colors;
        }

        private void UpdatePortraits()
        {
            UpdatePortrait(momoHero, momoPortraitText, momoPortraitImage, momoSelectedColor);
            UpdatePortrait(bulwarkHero, bulwarkPortraitText, bulwarkPortraitImage, bulwarkSelectedColor);
            UpdatePortrait(sproutHero, sproutPortraitText, sproutPortraitImage, sproutSelectedColor);
        }

        private void UpdatePortrait(PrototypeHeroController hero, Text label, Image portraitImage, Color selectedColor)
        {
            if (hero == null)
            {
                return;
            }

            if (label != null)
            {
                string selectedMarker = hero.IsSelected ? "*" : string.Empty;
                string persistentLevel = progressionService != null ? $" P{progressionService.GetHeroLevel(hero.HeroName)}" : string.Empty;
                label.text = $"{hero.HeroName} L{hero.Level}{persistentLevel}{selectedMarker}";
            }

            if (portraitImage != null)
            {
                if (hero.PortraitSprite != null)
                {
                    portraitImage.sprite = hero.PortraitSprite;
                    portraitImage.preserveAspect = true;
                    portraitImage.color = hero.IsSelected
                        ? Color.white
                        : new Color(1f, 1f, 1f, 0.82f);
                }
                else
                {
                    portraitImage.color = hero.IsSelected ? selectedColor : idlePortraitColor;
                }
            }
        }

        private void UpdateTowerButtons()
        {
            UpdateTowerButton(towerBuildText, towerBuildButton, 0);
            UpdateTowerButton(burstBuildText, burstBuildButton, 1);
            UpdateTowerButton(frostBuildText, frostBuildButton, 2);
        }

        private void UpdateTowerButton(Text label, Button button, int optionIndex)
        {
            if (label == null || button == null || buildManager == null || buildManager.BuildOptions == null || optionIndex >= buildManager.BuildOptions.Length)
            {
                return;
            }

            TowerBuildManager.TowerBuildOption option = buildManager.BuildOptions[optionIndex];
            if (option == null || option.towerDefinition == null)
            {
                label.text = "Unavailable";
                button.interactable = false;
                return;
            }

            label.text = $"{option.towerDefinition.DisplayName} {option.towerDefinition.BuildCost}g";
            ColorBlock colors = button.colors;
            bool isSelected = buildManager.SelectedOptionIndex == optionIndex;
            colors.normalColor = isSelected ? new Color(0.95f, 0.82f, 0.4f, 1f) : Color.white;
            colors.highlightedColor = isSelected ? new Color(1f, 0.9f, 0.55f, 1f) : new Color(1f, 0.92f, 0.96f, 1f);
            button.colors = colors;
            button.interactable = true;
        }

        private void StartNextWave()
        {
            if (!BattleSession.HasSeenTutorial)
            {
                BattleSession.MarkTutorialSeen();
            }

            PrototypeAudioDirector.PlayWaveStart();
            waveSpawner?.StartNextWave();
        }

        private void RestartFromShell()
        {
            BattleSession.RequestRestartFromShell();
            ReturnToShell();
        }

        private void ExitToShell()
        {
            BattleSession.CancelPendingShellRestart();
            ReturnToShell();
        }

        private void ReturnToShell()
        {
            string shellSceneName = BattleSession.ResolveShellReturnScene(fallbackShellSceneName);
            if (!string.IsNullOrWhiteSpace(shellSceneName) && Application.CanStreamedLevelBeLoaded(shellSceneName))
            {
                SceneManager.LoadScene(shellSceneName);
                return;
            }

            if (!string.IsNullOrWhiteSpace(fallbackShellSceneName) && Application.CanStreamedLevelBeLoaded(fallbackShellSceneName))
            {
                Debug.LogWarning($"Unable to load shell scene '{shellSceneName}'. Falling back to '{fallbackShellSceneName}'.");
                SceneManager.LoadScene(fallbackShellSceneName);
                return;
            }

            Debug.LogWarning($"Unable to load shell scene '{shellSceneName}' and fallback scene '{fallbackShellSceneName}' is not available.");
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

        private void UpdateAutoReturn(bool hasBattleEnded)
        {
            if (!autoReturnToShellOnBattleEnd || !hasBattleEnded)
            {
                queuedAutoReturnToShell = false;
                autoReturnTimer = 0f;
                return;
            }

            if (!queuedAutoReturnToShell)
            {
                queuedAutoReturnToShell = true;
                autoReturnTimer = Mathf.Max(0f, autoReturnDelaySeconds);
            }

            if (autoReturnTimer > 0f)
            {
                autoReturnTimer -= Time.deltaTime;
                return;
            }

            ExitToShell();
        }

        private void GrantVictoryReward()
        {
            if (grantedVictoryReward || progressionService == null)
            {
                return;
            }

            grantedVictoryReward = true;
            progressionService.GrantVictoryReward();

            if (rewardText != null)
            {
                rewardText.text = $"+{progressionService.VictoryCurrencyReward} crystals";
            }
        }

        private void ApplyPersistentProgression()
        {
            if (momoHero != null && progressionService != null)
            {
                momoHero.ApplyPersistentHeroLevel(progressionService.GetHeroLevel("Momo"));
                momoHero.ApplyPersistentSkillRank(progressionService.MomoSkillRank);
                momoHero.ApplyEquipmentBonus(progressionService.EquippedHeroSkillDamageBonus);
            }

            if (bulwarkHero != null && progressionService != null)
            {
                bulwarkHero.ApplyPersistentHeroLevel(progressionService.GetHeroLevel("Bulwark"));
                bulwarkHero.ApplyPersistentSkillRank(progressionService.GetHeroSkillRank("Bulwark"));
                bulwarkHero.ApplyEquipmentBonus(progressionService.EquippedHeroSkillDamageBonus);
            }

            if (sproutHero != null && progressionService != null)
            {
                sproutHero.ApplyPersistentHeroLevel(progressionService.GetHeroLevel("Sprout"));
                sproutHero.ApplyPersistentSkillRank(progressionService.GetHeroSkillRank("Sprout"));
                sproutHero.ApplyEquipmentBonus(progressionService.EquippedHeroSkillDamageBonus);
            }
        }

        private void UpdateObjective()
        {
            if (objectiveText == null)
            {
                return;
            }

            if (!BattleSession.HasSeenTutorial && waveSpawner != null && waveSpawner.CurrentWave == 0)
            {
                objectiveText.text = "Tutorial: build a tower on a node, select a hero, then start the wave.";
                return;
            }

            if (!BattleSession.HasSeenTutorial && heroSelection != null && heroSelection.SelectedHero != null && waveSpawner != null && waveSpawner.CurrentWave > 0)
            {
                objectiveText.text = $"Tutorial: reposition {heroSelection.SelectedHero.HeroName} and use {heroSelection.SelectedHero.SkillName}.";
                return;
            }

            string levelObjective = waveSpawner != null && waveSpawner.ActiveLevel != null
                ? waveSpawner.ActiveLevel.ObjectiveText
                : "Build towers. Start waves. Defend the path.";
            objectiveText.text = levelObjective;
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

        private void EnsureSkillButton()
        {
            if (skillButton != null && skillText != null)
            {
                return;
            }

            Canvas canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                return;
            }

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject buttonObject = new GameObject("Skill Button");
            buttonObject.transform.SetParent(canvas.transform);

            Image image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.94f, 0.58f, 0.74f, 0.9f);
            skillButton = buttonObject.AddComponent<Button>();

            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0f, 0f);
            buttonRect.anchorMax = new Vector2(0f, 0f);
            buttonRect.pivot = new Vector2(0f, 0f);
            buttonRect.anchoredPosition = new Vector2(16f, 86f);
            buttonRect.sizeDelta = new Vector2(220f, 54f);

            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(buttonObject.transform);
            skillText = labelObject.AddComponent<Text>();
            skillText.text = "Skill";
            skillText.font = font;
            skillText.fontSize = 22;
            skillText.color = Color.white;
            skillText.alignment = TextAnchor.MiddleCenter;
            skillText.raycastTarget = false;

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
        }
    }
}

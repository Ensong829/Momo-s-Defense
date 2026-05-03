using MomosDefense.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MomosDefense.UI
{
    public sealed class LevelSelectShellController : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [SerializeField] private string battleSceneName = "Prototype_MomoDefense";
        [SerializeField] private LevelDefinition defaultLevel;
        [SerializeField] private LevelSelectOptionButton[] levelButtons;

        public LevelDefinition SelectedLevel { get; private set; }

        private void Awake()
        {
            bool autoStartRequested = BattleSession.ConsumeAutoStartSelectedLevelOnShellEntry();

            if (levelButtons == null || levelButtons.Length == 0)
            {
                levelButtons = GetComponentsInChildren<LevelSelectOptionButton>(true);
            }

            for (int index = 0; index < levelButtons.Length; index++)
            {
                if (levelButtons[index] != null)
                {
                    levelButtons[index].Bind(this);
                }
            }

            LevelDefinition initialLevel = BattleSession.ResolveLevel(defaultLevel);
            if (initialLevel != null)
            {
                SelectLevel(initialLevel);
            }

            if (autoStartRequested)
            {
                StartSelectedLevel();
            }
        }

        public void SelectLevel(LevelDefinition level)
        {
            SelectedLevel = level;

            for (int index = 0; index < levelButtons.Length; index++)
            {
                if (levelButtons[index] != null)
                {
                    levelButtons[index].SetSelected(levelButtons[index].Level == SelectedLevel);
                }
            }
        }

        public void StartSelectedLevel()
        {
            if (SelectedLevel == null)
            {
                SelectedLevel = defaultLevel;
            }

            if (SelectedLevel == null)
            {
                Debug.LogWarning("Level start requested, but no level is selected and no default level is configured.");
                return;
            }

            BattleSession.CancelPendingShellRestart();
            BattleSession.SelectLevel(SelectedLevel);
            BattleSession.ConfigureBattleLaunch(SceneManager.GetActiveScene().name, battleSceneName);

            if (!string.IsNullOrEmpty(battleSceneName))
            {
                SceneManager.LoadScene(battleSceneName);
            }
        }

        public void BackToMainMenu()
        {
            BattleSession.ClearSelectedLevel();

            if (!string.IsNullOrEmpty(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
        }
    }
}

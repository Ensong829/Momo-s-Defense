using UnityEngine;

namespace MomosDefense.Core
{
    public static class BattleSession
    {
        private const string TutorialSeenKey = "MomosDefense.Tutorial.Seen";
        private const string DefaultShellSceneName = "LevelSelect";

        public static LevelDefinition SelectedLevel { get; private set; }
        public static string ShellReturnSceneName { get; private set; } = DefaultShellSceneName;
        public static string BattleSceneName { get; private set; } = "Prototype_MomoDefense";
        public static bool ShouldAutoStartSelectedLevelOnShellEntry { get; private set; }

        public static void SelectLevel(LevelDefinition levelDefinition)
        {
            SelectedLevel = levelDefinition;
        }

        public static void ConfigureBattleLaunch(string shellReturnSceneName, string battleSceneName)
        {
            if (!string.IsNullOrWhiteSpace(shellReturnSceneName))
            {
                ShellReturnSceneName = shellReturnSceneName;
            }

            if (!string.IsNullOrWhiteSpace(battleSceneName))
            {
                BattleSceneName = battleSceneName;
            }
        }

        public static LevelDefinition ResolveLevel(LevelDefinition fallbackLevel)
        {
            return SelectedLevel != null ? SelectedLevel : fallbackLevel;
        }

        public static string ResolveShellReturnScene(string fallbackSceneName = DefaultShellSceneName)
        {
            return !string.IsNullOrWhiteSpace(ShellReturnSceneName) ? ShellReturnSceneName : fallbackSceneName;
        }

        public static void RequestRestartFromShell()
        {
            ShouldAutoStartSelectedLevelOnShellEntry = SelectedLevel != null;
        }

        public static void CancelPendingShellRestart()
        {
            ShouldAutoStartSelectedLevelOnShellEntry = false;
        }

        public static bool ConsumeAutoStartSelectedLevelOnShellEntry()
        {
            bool shouldAutoStart = ShouldAutoStartSelectedLevelOnShellEntry;
            ShouldAutoStartSelectedLevelOnShellEntry = false;
            return shouldAutoStart;
        }

        public static void ClearSelectedLevel()
        {
            SelectedLevel = null;
            CancelPendingShellRestart();
        }

        public static bool HasSeenTutorial => PlayerPrefs.GetInt(TutorialSeenKey, 0) == 1;

        public static void MarkTutorialSeen()
        {
            PlayerPrefs.SetInt(TutorialSeenKey, 1);
            PlayerPrefs.Save();
        }
    }
}

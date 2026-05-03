using MomosDefense.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MomosDefense.UI
{
    public sealed class MainMenuShellController : MonoBehaviour
    {
        [SerializeField] private string levelSelectSceneName = "LevelSelect";
        [SerializeField] private string quickPlaySceneName = "Prototype_MomoDefense";
        [SerializeField] private bool clearSelectedLevelOnMenuEntry = true;

        private void Awake()
        {
            BattleSession.CancelPendingShellRestart();

            if (clearSelectedLevelOnMenuEntry)
            {
                BattleSession.ClearSelectedLevel();
            }
        }

        public void OpenLevelSelect()
        {
            if (!string.IsNullOrEmpty(levelSelectSceneName))
            {
                SceneManager.LoadScene(levelSelectSceneName);
            }
        }

        public void StartQuickPlay()
        {
            BattleSession.ClearSelectedLevel();
            BattleSession.ConfigureBattleLaunch(SceneManager.GetActiveScene().name, quickPlaySceneName);

            if (!string.IsNullOrEmpty(quickPlaySceneName))
            {
                SceneManager.LoadScene(quickPlaySceneName);
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

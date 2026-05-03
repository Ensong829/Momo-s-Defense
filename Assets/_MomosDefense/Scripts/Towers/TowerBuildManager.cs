using MomosDefense.Core;
using UnityEngine;

namespace MomosDefense.Towers
{
    public sealed class TowerBuildManager : MonoBehaviour
    {
        [System.Serializable]
        public sealed class TowerBuildOption
        {
            public TowerDefinition towerDefinition;
            public GameObject towerPrefab;
        }

        [SerializeField] private TowerBuildOption[] buildOptions;
        [SerializeField] private int startingOptionIndex;

        public TowerBuildOption[] BuildOptions => buildOptions;
        public int SelectedOptionIndex { get; private set; }
        public TowerBuildOption SelectedOption =>
            buildOptions != null && SelectedOptionIndex >= 0 && SelectedOptionIndex < buildOptions.Length
                ? buildOptions[SelectedOptionIndex]
                : null;

        private void Awake()
        {
            if (buildOptions == null || buildOptions.Length == 0)
            {
                SelectedOptionIndex = -1;
                return;
            }

            SelectedOptionIndex = Mathf.Clamp(startingOptionIndex, 0, buildOptions.Length - 1);
        }

        public void SelectOption(int optionIndex)
        {
            if (buildOptions == null || optionIndex < 0 || optionIndex >= buildOptions.Length)
            {
                return;
            }

            SelectedOptionIndex = optionIndex;
        }
    }
}

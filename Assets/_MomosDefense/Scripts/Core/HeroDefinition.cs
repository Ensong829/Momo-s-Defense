using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Hero Definition")]
    public sealed class HeroDefinition : ScriptableObject
    {
        [SerializeField] private string heroId = "Momo";
        [SerializeField] private string displayName = "Momo";
        [SerializeField] private string role = "Control";
        [SerializeField] private int startingLevel = 1;

        public string HeroId => heroId;
        public string DisplayName => displayName;
        public string Role => role;
        public int StartingLevel => startingLevel;
    }
}

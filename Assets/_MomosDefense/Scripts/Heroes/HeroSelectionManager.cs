using UnityEngine;

namespace MomosDefense.Heroes
{
    public sealed class HeroSelectionManager : MonoBehaviour
    {
        [SerializeField] private MomoHeroController[] heroes;
        [SerializeField] private MomoHeroController startingHero;

        public MomoHeroController SelectedHero { get; private set; }

        private void Awake()
        {
            if (heroes == null || heroes.Length == 0)
            {
                heroes = FindObjectsByType<MomoHeroController>(FindObjectsInactive.Exclude);
            }

            if (startingHero == null && heroes != null && heroes.Length > 0)
            {
                startingHero = heroes[0];
            }

            SelectHero(startingHero);
        }

        public void SelectHero(MomoHeroController hero)
        {
            if (hero == null)
            {
                return;
            }

            SelectedHero = hero;

            foreach (MomoHeroController selectableHero in heroes)
            {
                if (selectableHero != null)
                {
                    selectableHero.SetSelected(selectableHero == SelectedHero);
                }
            }
        }
    }
}

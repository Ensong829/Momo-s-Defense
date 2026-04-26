using UnityEngine;

namespace MomosDefense.Heroes
{
    public sealed class HeroSelectionManager : MonoBehaviour
    {
        [SerializeField] private PrototypeHeroController[] heroes;
        [SerializeField] private PrototypeHeroController startingHero;

        public PrototypeHeroController[] Heroes => heroes;
        public PrototypeHeroController SelectedHero { get; private set; }

        private void Awake()
        {
            if (heroes == null || heroes.Length == 0)
            {
                heroes = FindObjectsByType<PrototypeHeroController>(FindObjectsInactive.Exclude);
            }

            if (startingHero == null && heroes != null && heroes.Length > 0)
            {
                startingHero = heroes[0];
            }

            SelectHero(startingHero);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectHeroAtIndex(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectHeroAtIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectHeroAtIndex(2);
            }
        }

        public void SelectHero(PrototypeHeroController hero)
        {
            if (hero == null)
            {
                return;
            }

            SelectedHero = hero;

            foreach (PrototypeHeroController selectableHero in heroes)
            {
                if (selectableHero != null)
                {
                    selectableHero.SetSelected(selectableHero == SelectedHero);
                }
            }
        }

        private void SelectHeroAtIndex(int index)
        {
            if (heroes == null || index < 0 || index >= heroes.Length)
            {
                return;
            }

            SelectHero(heroes[index]);
        }
    }
}

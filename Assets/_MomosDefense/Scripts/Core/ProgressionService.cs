using UnityEngine;

namespace MomosDefense.Core
{
    public sealed class ProgressionService : MonoBehaviour
    {
        private const string SoftCurrencyKey = "MomosDefense.SoftCurrency";
        private const string HeroSkillRankPrefix = "MomosDefense.HeroSkillRank.";
        private const string HeroLevelPrefix = "MomosDefense.HeroLevel.";
        private const string TowerRankPrefix = "MomosDefense.TowerRank.";
        private const string TowerSpecializationPrefix = "MomosDefense.TowerSpecialization.";
        private const string EquippedWeaponKey = "MomosDefense.Equipment.Weapon";
        private const string EquippedCharmKey = "MomosDefense.Equipment.Charm";
        private const string EquippedRelicKey = "MomosDefense.Equipment.Relic";
        private const string StarterCharmId = "TrainingCharm";
        private const int StartingRank = 1;
        private const int MaxRank = 5;

        [SerializeField] private int fallbackVictoryCurrencyReward = 35;
        [SerializeField] private UpgradeDefinition heroSkillUpgradeDefinition;
        [SerializeField] private UpgradeDefinition towerFamilyUpgradeDefinition;
        [SerializeField] private EquipmentDefinition starterCharmDefinition;

        public int SoftCurrency { get; private set; }
        public int MomoSkillRank => GetHeroSkillRank("Momo");
        public int VictoryCurrencyReward => BattleSession.ResolveLevel(null) != null
            ? BattleSession.ResolveLevel(null).VictoryCurrencyReward
            : fallbackVictoryCurrencyReward;
        public int MaxSkillRank => heroSkillUpgradeDefinition != null ? heroSkillUpgradeDefinition.MaxRank : MaxRank;
        public bool CanUpgradeMomoSkill => CanUpgradeHeroSkill("Momo");
        public int MomoSkillUpgradeCost => GetHeroSkillUpgradeCost("Momo");
        public string EquippedWeapon { get; private set; }
        public string EquippedCharm { get; private set; }
        public string EquippedRelic { get; private set; }
        public int EquippedHeroSkillDamageBonus =>
            EquippedCharm == GetStarterCharmId() && starterCharmDefinition != null ? starterCharmDefinition.HeroSkillDamageBonus : 0;
        public float EquippedTowerAttackSpeedBonus =>
            EquippedCharm == GetStarterCharmId() && starterCharmDefinition != null ? starterCharmDefinition.TowerAttackSpeedBonus : 0f;

        private readonly string[] heroIds = { "Momo", "Bulwark", "Sprout" };
        private readonly string[] towerFamilyIds = { "Star", "Burst", "Frost" };
        private readonly int[] heroLevels = new int[3];
        private readonly int[] heroSkillRanks = new int[3];
        private readonly int[] towerFamilyRanks = new int[3];
        private readonly string[] towerSpecializations = new string[3];

        private void Awake()
        {
            Load();
        }

        public void GrantVictoryReward()
        {
            AddSoftCurrency(VictoryCurrencyReward);
        }

        public int GetHeroSkillRank(string heroId)
        {
            int index = IndexOf(heroIds, heroId);
            return index >= 0 ? heroSkillRanks[index] : StartingRank;
        }

        public int GetHeroLevel(string heroId)
        {
            int index = IndexOf(heroIds, heroId);
            return index >= 0 ? heroLevels[index] : StartingRank;
        }

        public int GetTowerFamilyRank(string familyId)
        {
            int index = IndexOf(towerFamilyIds, familyId);
            return index >= 0 ? towerFamilyRanks[index] : StartingRank;
        }

        public string GetTowerSpecialization(string familyId)
        {
            int index = IndexOf(towerFamilyIds, familyId);
            return index >= 0 ? towerSpecializations[index] : string.Empty;
        }

        public int GetHeroSkillUpgradeCost(string heroId)
        {
            int rank = GetHeroSkillRank(heroId);
            return rank >= MaxRank
                ? 0
                : GetUpgradeCost(heroSkillUpgradeDefinition, rank);
        }

        public int GetTowerFamilyUpgradeCost(string familyId)
        {
            int rank = GetTowerFamilyRank(familyId);
            return rank >= MaxRank
                ? 0
                : GetUpgradeCost(towerFamilyUpgradeDefinition, rank);
        }

        public bool CanUpgradeHeroSkill(string heroId)
        {
            return GetHeroSkillRank(heroId) < MaxRank && SoftCurrency >= GetHeroSkillUpgradeCost(heroId);
        }

        public bool CanUpgradeTowerFamily(string familyId)
        {
            return GetTowerFamilyRank(familyId) < MaxRank && SoftCurrency >= GetTowerFamilyUpgradeCost(familyId);
        }

        public bool CanChooseTowerSpecialization(string familyId)
        {
            return GetTowerFamilyRank(familyId) >= 3 && string.IsNullOrEmpty(GetTowerSpecialization(familyId));
        }

        public bool TryUpgradeMomoSkill()
        {
            return TryUpgradeHeroSkill("Momo");
        }

        public bool TryUpgradeHeroSkill(string heroId)
        {
            if (!CanUpgradeHeroSkill(heroId))
            {
                return false;
            }

            int index = IndexOf(heroIds, heroId);
            if (index < 0)
            {
                return false;
            }

            SoftCurrency -= GetHeroSkillUpgradeCost(heroId);
            heroSkillRanks[index]++;
            heroLevels[index] = Mathf.Max(heroLevels[index], heroSkillRanks[index]);
            Save();
            return true;
        }

        public bool TryUpgradeTowerFamily(string familyId)
        {
            if (!CanUpgradeTowerFamily(familyId))
            {
                return false;
            }

            int index = IndexOf(towerFamilyIds, familyId);
            if (index < 0)
            {
                return false;
            }

            SoftCurrency -= GetTowerFamilyUpgradeCost(familyId);
            towerFamilyRanks[index]++;
            Save();
            return true;
        }

        public bool TryChooseTowerSpecialization(string familyId)
        {
            if (!CanChooseTowerSpecialization(familyId))
            {
                return false;
            }

            int index = IndexOf(towerFamilyIds, familyId);
            if (index < 0)
            {
                return false;
            }

            towerSpecializations[index] = familyId switch
            {
                "Star" => "Focus",
                "Burst" => "Volley",
                "Frost" => "Deep Freeze",
                _ => "Specialized"
            };
            Save();
            return true;
        }

        public void ResetPrototypeProgression()
        {
            SoftCurrency = 0;
            for (int index = 0; index < heroSkillRanks.Length; index++)
            {
                heroLevels[index] = StartingRank;
                heroSkillRanks[index] = StartingRank;
            }

            for (int index = 0; index < towerFamilyRanks.Length; index++)
            {
                towerFamilyRanks[index] = StartingRank;
                towerSpecializations[index] = string.Empty;
            }

            EquippedWeapon = string.Empty;
            EquippedCharm = GetStarterCharmId();
            EquippedRelic = string.Empty;
            Save();
        }

        private void AddSoftCurrency(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            SoftCurrency += amount;
            Save();
        }

        private void Load()
        {
            SoftCurrency = Mathf.Max(0, PlayerPrefs.GetInt(SoftCurrencyKey, 0));
            for (int index = 0; index < heroIds.Length; index++)
            {
                heroLevels[index] = LoadRank(HeroLevelPrefix + heroIds[index]);
                heroSkillRanks[index] = LoadRank(HeroSkillRankPrefix + heroIds[index]);
                heroLevels[index] = Mathf.Max(heroLevels[index], heroSkillRanks[index]);
            }

            for (int index = 0; index < towerFamilyIds.Length; index++)
            {
                towerFamilyRanks[index] = LoadRank(TowerRankPrefix + towerFamilyIds[index]);
                towerSpecializations[index] = PlayerPrefs.GetString(TowerSpecializationPrefix + towerFamilyIds[index], string.Empty);
            }

            EquippedWeapon = PlayerPrefs.GetString(EquippedWeaponKey, string.Empty);
            EquippedCharm = PlayerPrefs.GetString(EquippedCharmKey, GetStarterCharmId());
            EquippedRelic = PlayerPrefs.GetString(EquippedRelicKey, string.Empty);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(SoftCurrencyKey, SoftCurrency);
            for (int index = 0; index < heroIds.Length; index++)
            {
                PlayerPrefs.SetInt(HeroLevelPrefix + heroIds[index], heroLevels[index]);
                PlayerPrefs.SetInt(HeroSkillRankPrefix + heroIds[index], heroSkillRanks[index]);
            }

            for (int index = 0; index < towerFamilyIds.Length; index++)
            {
                PlayerPrefs.SetInt(TowerRankPrefix + towerFamilyIds[index], towerFamilyRanks[index]);
                PlayerPrefs.SetString(TowerSpecializationPrefix + towerFamilyIds[index], towerSpecializations[index]);
            }

            PlayerPrefs.SetString(EquippedWeaponKey, EquippedWeapon);
            PlayerPrefs.SetString(EquippedCharmKey, EquippedCharm);
            PlayerPrefs.SetString(EquippedRelicKey, EquippedRelic);
            PlayerPrefs.Save();
        }

        private static int LoadRank(string key)
        {
            return Mathf.Clamp(PlayerPrefs.GetInt(key, StartingRank), StartingRank, MaxRank);
        }

        private int GetUpgradeCost(UpgradeDefinition definition, int currentRank)
        {
            if (definition == null)
            {
                return 0;
            }

            return definition.BaseCost + ((currentRank - StartingRank) * definition.CostStep);
        }

        private string GetStarterCharmId()
        {
            return starterCharmDefinition != null ? starterCharmDefinition.EquipmentId : StarterCharmId;
        }

        private static int IndexOf(string[] ids, string id)
        {
            for (int index = 0; index < ids.Length; index++)
            {
                if (ids[index] == id)
                {
                    return index;
                }
            }

            return -1;
        }
    }
}

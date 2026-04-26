using UnityEditor;
using UnityEngine;

namespace MomosDefense.Editor
{
    public static class PrototypeBalanceTools
    {
        private const string SoftCurrencyKey = "MomosDefense.SoftCurrency";
        private const string HeroSkillRankPrefix = "MomosDefense.HeroSkillRank.";
        private const string HeroLevelPrefix = "MomosDefense.HeroLevel.";
        private const string TowerRankPrefix = "MomosDefense.TowerRank.";
        private const string TowerSpecializationPrefix = "MomosDefense.TowerSpecialization.";
        private static readonly string[] HeroIds = { "Momo", "Bulwark", "Sprout" };
        private static readonly string[] TowerIds = { "Star", "Burst", "Frost" };

        [MenuItem("Momo's Defense/Debug/Grant 500 Crystals")]
        public static void GrantCrystals()
        {
            PlayerPrefs.SetInt(SoftCurrencyKey, PlayerPrefs.GetInt(SoftCurrencyKey, 0) + 500);
            PlayerPrefs.Save();
            Debug.Log("Granted 500 prototype crystals.");
        }

        [MenuItem("Momo's Defense/Debug/Max Prototype Progression")]
        public static void MaxPrototypeProgression()
        {
            PlayerPrefs.SetInt(SoftCurrencyKey, 9999);

            foreach (string heroId in HeroIds)
            {
                PlayerPrefs.SetInt(HeroSkillRankPrefix + heroId, 5);
                PlayerPrefs.SetInt(HeroLevelPrefix + heroId, 5);
            }

            foreach (string towerId in TowerIds)
            {
                PlayerPrefs.SetInt(TowerRankPrefix + towerId, 5);
                PlayerPrefs.SetString(TowerSpecializationPrefix + towerId, towerId switch
                {
                    "Star" => "Focus",
                    "Burst" => "Volley",
                    "Frost" => "Deep Freeze",
                    _ => "Specialized"
                });
            }

            PlayerPrefs.Save();
            Debug.Log("Maxed prototype progression.");
        }

        [MenuItem("Momo's Defense/Debug/Reset Prototype Progression")]
        public static void ResetPrototypeProgression()
        {
            PlayerPrefs.DeleteKey(SoftCurrencyKey);

            foreach (string heroId in HeroIds)
            {
                PlayerPrefs.DeleteKey(HeroSkillRankPrefix + heroId);
                PlayerPrefs.DeleteKey(HeroLevelPrefix + heroId);
            }

            foreach (string towerId in TowerIds)
            {
                PlayerPrefs.DeleteKey(TowerRankPrefix + towerId);
                PlayerPrefs.DeleteKey(TowerSpecializationPrefix + towerId);
            }

            PlayerPrefs.Save();
            Debug.Log("Reset prototype progression.");
        }
    }
}

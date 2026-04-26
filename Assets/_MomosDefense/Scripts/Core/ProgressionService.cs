using UnityEngine;

namespace MomosDefense.Core
{
    public sealed class ProgressionService : MonoBehaviour
    {
        private const string SoftCurrencyKey = "MomosDefense.SoftCurrency";
        private const string MomoSkillRankKey = "MomosDefense.MomoSkillRank";
        private const int StartingMomoSkillRank = 1;
        private const int MaxMomoSkillRank = 5;

        [SerializeField] private int victoryCurrencyReward = 35;
        [SerializeField] private int baseMomoSkillUpgradeCost = 50;
        [SerializeField] private int momoSkillUpgradeCostStep = 35;

        public int SoftCurrency { get; private set; }
        public int MomoSkillRank { get; private set; }
        public int VictoryCurrencyReward => victoryCurrencyReward;
        public int MaxSkillRank => MaxMomoSkillRank;
        public bool CanUpgradeMomoSkill => MomoSkillRank < MaxMomoSkillRank && SoftCurrency >= MomoSkillUpgradeCost;
        public int MomoSkillUpgradeCost => MomoSkillRank >= MaxMomoSkillRank
            ? 0
            : baseMomoSkillUpgradeCost + ((MomoSkillRank - StartingMomoSkillRank) * momoSkillUpgradeCostStep);

        private void Awake()
        {
            Load();
        }

        public void GrantVictoryReward()
        {
            AddSoftCurrency(victoryCurrencyReward);
        }

        public bool TryUpgradeMomoSkill()
        {
            if (!CanUpgradeMomoSkill)
            {
                return false;
            }

            SoftCurrency -= MomoSkillUpgradeCost;
            MomoSkillRank++;
            Save();
            return true;
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
            MomoSkillRank = Mathf.Clamp(PlayerPrefs.GetInt(MomoSkillRankKey, StartingMomoSkillRank), StartingMomoSkillRank, MaxMomoSkillRank);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(SoftCurrencyKey, SoftCurrency);
            PlayerPrefs.SetInt(MomoSkillRankKey, MomoSkillRank);
            PlayerPrefs.Save();
        }
    }
}

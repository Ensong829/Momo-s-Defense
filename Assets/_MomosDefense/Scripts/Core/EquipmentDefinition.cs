using UnityEngine;

namespace MomosDefense.Core
{
    [CreateAssetMenu(menuName = "Momo's Defense/Equipment Definition")]
    public sealed class EquipmentDefinition : ScriptableObject
    {
        public enum EquipmentSlot
        {
            Weapon,
            Charm,
            Relic
        }

        [SerializeField] private string equipmentId = "TrainingCharm";
        [SerializeField] private string displayName = "Training Charm";
        [SerializeField] private EquipmentSlot slot = EquipmentSlot.Charm;
        [SerializeField] private int heroSkillDamageBonus;
        [SerializeField] private float towerAttackSpeedBonus;
        [SerializeField] private string specialModifier = "None";

        public string EquipmentId => equipmentId;
        public string DisplayName => displayName;
        public EquipmentSlot Slot => slot;
        public int HeroSkillDamageBonus => heroSkillDamageBonus;
        public float TowerAttackSpeedBonus => towerAttackSpeedBonus;
        public string SpecialModifier => specialModifier;
    }
}

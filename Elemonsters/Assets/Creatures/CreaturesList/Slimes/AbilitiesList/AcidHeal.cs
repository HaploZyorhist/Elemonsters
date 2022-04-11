using Elemonsters.Assets.Creatures.CreaturesList.Slimes.PassiveAbilities;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.CreaturesList.Slimes.AbilitiesList
{
    /// <summary>
    /// acid heal, causes user to heal when acid damage is dealt
    /// </summary>
    public class AcidHeal : Ability
    {
        /// <summary>
        /// ctor for the ability
        /// </summary>
        public AcidHeal()
        {
            Name = "Acid Heal";
            IsActive = false;
            AbilitySlot = AbilitySlotEnum.Passive;

            var newPassive = new AcidHealPassive();

            PassiveAbilities.Add(newPassive);
        }
    }
}

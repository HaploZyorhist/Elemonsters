using Elemonsters.Assets.Creatures.PassiveAbilities;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.AbilitiesList
{
    public class Blight : Ability
    {
        public Blight()
        {
            Name = "Blight";
            AbilityLevel = 0;
            IsActive = false;
            AbilitySlot = AbilitySlot.SecondAbility;

            var newPassive = new TestPassive();

            PassiveAbilities.Add(newPassive);
        }
    }
}

using Elemonsters.Assets.Creatures.PassiveAbilities;
using Elemonsters.Models.Combat.Results;
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


        /// <inheritdoc />
        public override async Task<TargetRulesResult> GetTargetOptions()
        {
            try
            {
                // create result object
                var result = new TargetRulesResult
                {
                    Rule = TargetingRulesEnum.Self,
                    TotalTargets = 1,
                    UniqueTargets = true
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

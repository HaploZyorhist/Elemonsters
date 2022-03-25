using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;
using Elemonsters.Models.StatusEffects.Results;

namespace Elemonsters.Assets.Creatures.AbilitiesList
{
    /// <summary>
    /// ability for adding shields to the creature
    /// </summary>
    public class GenerateShield : Ability
    {
        /// <summary>
        /// base constructor for the ability
        /// </summary>
        public GenerateShield()
        {
            Name = "Total Defense";
            AbilityLevel = 0;
            IsActive = true;
            AbilitySlot = AbilitySlot.FirstAbility;
        }

        /// <inheritdoc />
        public override async Task<ActiveResult> Activation(ActiveRequest request)
        {
            try
            {
                // create return object
                var result = new ActiveResult();

                var statusEffectResults = new AddStatusEffectResult();

                var me = request.Container.Creatures.Where(x => x.CreatureID == request.MyTurn)
                    .FirstOrDefault();

                var statuses = new List<BuffDebuff>();

                var shield = new BuffDebuff
                {
                    Name = "Shield",
                    IsBuff = true,
                    Duration = 3,
                    Stacks = 1,
                    Level = AbilityLevel,
                    Value = 200 + 150 * AbilityLevel,
                    EffectType = EffectTypes.GeneralShield,
                };

                statuses.Add(shield);

                var physicalShield = new BuffDebuff
                {
                    Name = "Physical Shield",
                    IsBuff = true,
                    Duration = 3,
                    Stacks = 1,
                    Level = AbilityLevel,
                    Value = 100 + 200 * AbilityLevel,
                    EffectType = EffectTypes.PhysicalShield,
                };

                statuses.Add(physicalShield);

                var elementalShield = new BuffDebuff
                {
                    Name = "Elemental",
                    IsBuff = true,
                    Duration = 3,
                    Stacks = 1,
                    Level = AbilityLevel,
                    Value = 300 + 50 * AbilityLevel,
                    EffectType = EffectTypes.ElementalShield,
                };

                statuses.Add(elementalShield);

                me.Statuses.AddRange(statuses);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override async Task<TargetRulesResult> GetTargetOptions()
        {
            try
            {
                var result = new TargetRulesResult
                {
                    Rule = TargetingRulesEnum.NoTarget,
                    TotalTargets = 0,
                    UniqueTargets = true,
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

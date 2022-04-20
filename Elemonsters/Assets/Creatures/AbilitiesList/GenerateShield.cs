using System.Text;
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
            IsActive = true;
            AbilitySlot = AbilitySlotEnum.FirstAbility;
        }

        /// <inheritdoc />
        public override async Task<ActiveAbilityResult> Activation(ActiveRequest request)
        {
            try
            {
                // create return object
                var result = new ActiveAbilityResult();

                var me = request.Creatures.Where(x => x.CreatureID == request.MyTurn)
                    .FirstOrDefault();

                var statuses = new List<BuffDebuff>();

                var shield = new BuffDebuff
                {
                    Name = "Shield",
                    IsBuff = true,
                    Duration = 3,
                    Stacks = 1,
                    Level = (int)me.Rank,
                    Value = 200 + 150 * (int)me.Rank,
                    EffectType = EffectTypesEnum.GeneralShield,
                };

                result.Messages.AppendLine($"<@{me.User}>'s {me.Name} has gained {shield.Value} {shield.Name}");

                statuses.Add(shield);

                var physicalShield = new BuffDebuff
                {
                    Name = "Physical Shield",
                    IsBuff = true,
                    Duration = 3,
                    Stacks = 1,
                    Level = (int)me.Rank,
                    Value = 100 + 200 * (int)me.Rank,
                    EffectType = EffectTypesEnum.PhysicalShield,
                };

                result.Messages.AppendLine($"<@{me.User}>'s {me.Name} has gained {physicalShield.Value} {physicalShield.Name}");

                statuses.Add(physicalShield);

                var elementalShield = new BuffDebuff
                {
                    Name = "Elemental",
                    IsBuff = true,
                    Duration = 3,
                    Stacks = 1,
                    Level = (int)me.Rank,
                    Value = 300 + 50 * (int)me.Rank,
                    EffectType = EffectTypesEnum.ElementalShield,
                };

                result.Messages.AppendLine($"<@{me.User}>'s {me.Name} has gained {elementalShield.Value} {elementalShield.Name}");

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
                    Rule = TargetingRulesEnum.Self,
                    TotalTargets = 1,
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

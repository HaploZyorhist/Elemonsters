using Elemonsters.Assets.Passives;
using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Enums;
using Elemonsters.Models.StatusEffects.Requests;
using Elemonsters.Models.StatusEffects.Results;

namespace Elemonsters.Assets.Creatures.PassiveAbilities
{
    /// <summary>
    /// this is a test passive for adding magic damage to an attack
    /// </summary>
    public class TestPassive : PassiveAbility
    {
        /// <inheritdoc />
        public override async Task<AddStatusEffectResult> AddStatusEffect(AddStatusEffectRequest request)
        {
            var result = new AddStatusEffectResult();

            foreach (var target in request.Targets)
            {
                var targetCreature = request.Creatures.Where(x => x.CreatureID == target).FirstOrDefault();

                var activatingCreatures = new List<ulong>();
                activatingCreatures.Add(target);

                var newPassive = new TestPassiveBuff
                {
                    Name = "Blight",
                    IsBuff = true,
                    Duration = 1,
                    Stacks = 0,
                    Value = 0,
                    Level = request.Level,
                    TriggerConditions = TriggerConditionsEnum.OnHit,
                    Stat = StatEffectedEnum.None,
                    ActivatingCreatures = activatingCreatures
                };

                targetCreature.Statuses.Add(newPassive);

                result.SB.AppendLine($"<@{targetCreature.User}>'s {targetCreature.Name} has gained {newPassive.Name}");
            }

            return result;
        }
    }
}

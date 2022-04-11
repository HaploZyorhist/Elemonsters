using Elemonsters.Assets.Creatures.CreaturesList.Slimes.StatusEffects;
using Elemonsters.Assets.Passives;
using Elemonsters.Models.Enums;
using Elemonsters.Models.StatusEffects.Requests;
using Elemonsters.Models.StatusEffects.Results;

namespace Elemonsters.Assets.Creatures.CreaturesList.Slimes.PassiveAbilities
{
    /// <summary>
    /// 
    /// </summary>
    public class AcidHealPassive : PassiveAbility
    {
        public override async Task<AddStatusEffectResult> AddStatusEffect(AddStatusEffectRequest request)
        {
            try
            {
                var result = new AddStatusEffectResult();

                foreach (var target in request.Targets)
                {
                    var targetCreature = request.Creatures.Where(x => x.CreatureID == target).FirstOrDefault();

                    var newPassive = new AcidHealBuff()
                    {
                        Name = "Acid Heal",
                        IsBuff = true,
                        Duration = 1,
                        Stacks = 0,
                        Value = 0,
                        Level = request.Level,
                        TriggerConditions = TriggerConditionsEnum.None,
                        Stat = StatEffectedEnum.None,
                    };

                    targetCreature.Statuses.Add(newPassive);

                    result.SB.AppendLine(
                        $"<@{targetCreature.User}>'s {targetCreature.Name} has gained {newPassive.Name}");
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

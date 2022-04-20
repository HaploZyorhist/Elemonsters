using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.CreaturesList.Slimes.StatusEffects
{
    public class AcidHealBuff : BuffDebuff
    {
        public AcidHealBuff()
        {
            Name = "Acid";
            IsBuff = false;
            Duration = 3;
            Stacks = 1;
            TriggerConditions = TriggerConditionsEnum.TurnEnd;
        }

        public override async Task<StatusEffectResult> ActivateEffect(ActivateStatusEffectRequest request)
        {
            try
            {
                var result = new StatusEffectResult();

                var healedCreatures = request.Creatures.Where(x =>
                    x.Statuses.Any(x => 
                                   x.Name.Equals("Acid Heal", StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                foreach (var healed in healedCreatures)
                {
                    var damageRequest = new DamageRequest();

                    damageRequest.Target = healed.CreatureID;
                    damageRequest.Damage = request.Value;
                    damageRequest.AttackType = AttackTypeEnum.Heal;
                    damageRequest.Penetration = 0;
                    damageRequest.TriggerCondition = TriggerConditionsEnum.None;

                    result.DamageRequests.Add(damageRequest);
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

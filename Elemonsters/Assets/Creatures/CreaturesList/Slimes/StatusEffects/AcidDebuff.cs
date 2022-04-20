using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Elemonsters.Assets.Creatures.CreaturesList.Slimes.StatusEffects
{
    public class AcidDebuff : BuffDebuff
    {
        public AcidDebuff()
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
                // create new result object
                var result = new StatusEffectResult();

                var me = request.Creatures.Where(x => x.CreatureID == request.MyTurn)
                    .FirstOrDefault();

                var activatingSlime = request.Creatures.Where(x => x.User == me.User && x.Species == "Slime")
                    .OrderBy(x => x.GetCurrentAura())
                    .FirstOrDefault();

                foreach (var target in request.Targets)
                {
                    // start new damage request to pass out to result
                    var damageRequest = new DamageRequest
                    {
                        Target = target,
                        TriggerCondition = TriggerConditionsEnum.None
                    };

                    // what kind of attack is being given
                    damageRequest.AttackType = AttackTypeEnum.Elemental;

                    // how much penetration is in the attack
                    damageRequest.Penetration = await activatingSlime.GetCurrentSorcery();

                    // how much the damage stat is modified by
                    double damageModifier = .35 + (Level * .05);

                    // calculate damage delt
                    var damageDealt = await activatingSlime.GetCurrentAura() * damageModifier;

                    // pass it to damage request
                    damageRequest.Damage = (int)damageDealt;

                    // add the damage request to the return object
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

using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;
using Elemonsters.Models.StatusEffects.Requests;
using Elemonsters.Models.StatusEffects.Results;

namespace Elemonsters.Assets.StatusEffects
{
    /// <summary>
    /// object for testing out passive effects
    /// </summary>
    internal class TestPassiveBuff : BuffDebuff
    {
        /// <summary>
        /// ctor for building a test passive
        /// </summary>
        public TestPassiveBuff()
        {
            Name = "Blight";
            IsBuff = true;
            Duration = 0;
            Stacks = 0;
            Value = 0;
            Level = 0;
            TriggerConditions = TriggerConditions.OnHit;
            Stat = StatEffectedEnum.None;
        }

        /// <inheritdoc />
        public override async Task<PassiveResult> ActivateEffect(ActivateStatusEffectRequest request)
        {
            try
            {
                // create new result object
                var result = new PassiveResult();

                var me = request.Container.Creatures.Where(x => x.CreatureID == request.MyTurn).FirstOrDefault();

                foreach (var target in request.Targets)
                {
                    // start new damage request to pass out to result
                    var damageRequest = new DamageRequest
                    {
                        ActiveCreature = request.MyTurn,
                        Target = target,
                        TriggerCondition = TriggerConditions.None
                    };

                    // what kind of attack is being given
                    damageRequest.AttackType = AttackTypeEnum.Magic;

                    // how much penetration is in the attack
                    damageRequest.Penetration = await me.GetCurrentSorcery();

                    // how much the damage stat is modified by
                    double damageModifier = .35 + (Level * .05);

                    // calculate damage delt
                    var damageDealt = await me.GetCurrentAura() * damageModifier;

                    // pass it to damage request
                    damageRequest.Damage = (int) damageDealt;

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

        /// <inheritdoc />
        public override async Task<CreatureBase> ReduceDuration(ReduceDurationRequest request)
        {
            return null;
        }
    }
}
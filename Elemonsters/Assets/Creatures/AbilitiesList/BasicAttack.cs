using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.AbilitiesList
{
    /// <summary>
    /// object containing details on a basic attack
    /// </summary>
    public class BasicAttack : Ability
    {
        /// <summary>
        /// base constructor for a basic attack
        /// </summary>
        public BasicAttack()
        {
            Name = "Basic Attack";
            AbilityLevel = 1;
            IsActive = true;
            AbilitySlot = AbilitySlotEnum.BasicAttack;
        }

        /// <inheritdoc />
        public override async Task<ActiveResult> Activation(ActiveRequest request)
        {
            try
            {
                // cannot perform an attack against nothing
                if (request.Targets.Count == 0)
                {
                    throw new Exception("There were no targets to select from");
                }

                // create result object
                var results = new ActiveResult();

                // only hits the selected target
                foreach (var t in request.Targets)
                {
                    var target = request.Container.Creatures.Where(x => x.CreatureID == t).FirstOrDefault();
                    var me = request.Container.Creatures.Where(x => x.CreatureID == request.MyTurn).FirstOrDefault();

                    // create object containing data on damage to be dealt
                    var damageRequest = new DamageRequest
                    {
                        ActiveCreature = request.MyTurn,
                        Target = target.CreatureID,
                        TriggerCondition = TriggerConditionsEnum.OnHit
                    };

                    // physical attack, uses physical attack stats
                    damageRequest.AttackType = AttackTypeEnum.Physical;

                    // how much defense is being penetrated
                    damageRequest.Penetration = target.Stats.Lethality;

                    // damage isn't modified
                    double damageModifier = 1;

                    // basic attacks can crit
                    var rand = new Random();
                    var r = rand.Next(0, 100);

                    // check for dodge first
                    if (r < target.Stats.Dodge)
                    {
                        results.SB.AppendLine($"<@{me.User}>'s {me.Name}'s attack has missed");
                        return results;
                    }

                    r = rand.Next(90, 100);

                    if (r < me.Stats.CritChance)
                    {
                        damageModifier = 1.5;
                        results.SB.AppendLine(
                            $"<@{me.User}>'s {me.Name} has landed a critical hit");
                    }

                    // add in on hit effects
                    var onHits = me.Statuses.Where(x => x.TriggerConditions == TriggerConditionsEnum.OnHit).ToList();

                    foreach (var effect in onHits)
                    {
                        var targets = new List<ulong>();

                        targets.Add(target.CreatureID);

                        var effectRequest = new ActivateStatusEffectRequest
                        {
                            Container = request.Container,
                            MyTurn = request.MyTurn,
                            Targets = targets
                        };

                        var activationResult = await effect.ActivateEffect(effectRequest);

                        results.DamageRequests.AddRange(activationResult.DamageRequests);
                    }

                    // calculates damage after crit
                    var damageDouble = me.Stats.Strength * damageModifier;

                    damageRequest.Damage = (int) damageDouble;

                    // adds damage request to result object
                    results.DamageRequests.Add(damageRequest);
                }

                return results;
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
                // create result object
                var result = new TargetRulesResult
                {
                    Rule = TargetingRulesEnum.Standard,
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

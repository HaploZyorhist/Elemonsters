using System.Data;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.ActiveAbilities
{
    /// <summary>
    /// Ability for performing basic physical attacks
    /// </summary>
    public class BasicAttackAbility : ActiveAbility
    {
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

                // only hits the selected target
                var target = request.Targets.First();

                // create result object
                var results = new ActiveResult();

                // create object containing data on damage to be dealt
                var damageRequest = new DamageRequest
                {
                    ActiveCreature = request.MyTurn.CreatureID,
                    Target = target.CreatureID
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
                if (r < request.Targets[0].Stats.Dodge)
                {
                    results.SB.AppendLine($"<@{request.MyTurn.User}>'s {request.MyTurn.Name}'s attack has missed");
                    return results;
                }

                r = rand.Next(90, 100);

                if (r < request.MyTurn.Stats.CritChance)
                {
                    damageModifier = 1.5;
                    results.SB.AppendLine($"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has landed a critical hit");
                }

                // calculates damage after crit
                var damageDouble = request.MyTurn.Stats.Strength * damageModifier;

                damageRequest.Damage = (int)damageDouble;

                // adds damage request to result object
                results.DamageRequests.Add(damageRequest);

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

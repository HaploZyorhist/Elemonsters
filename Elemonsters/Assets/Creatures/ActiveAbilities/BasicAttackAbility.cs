using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Factories;
using Elemonsters.Models.Combat;
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
                // only hits the selected target
                var target = request.Targets.First();

                // create result object
                var results = new ActiveResult();

                // create object containing data on damage to be dealt
                var damageRequest = new DamageRequest
                {
                    ActiveCreature = request.MyTurn,
                    Target = target
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

                if (r < request.MyTurn.Stats.CritChance)
                {
                    damageModifier = 1.5;
                    damageRequest.SB.AppendLine($"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has landed a critical hit");
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
        public override async Task<GetTargetsResult> GetTargetOptions(GetTargetsRequest request)
        {
            try
            {
                // create result object
                var result = new GetTargetsResult
                {
                    TotalTargets = 1,
                    FirstOptionTargets = 1
                };

                // create list of available targets
                List<CreatureBase> selectedTargets = new List<CreatureBase>();

                // can only select from the most front line targets
                var targets = request.Targets.Where(x => x.Position == PositionEnum.Melee).ToList();

                if (targets.Count == 0)
                {
                    targets = request.Targets.Where(x => x.Position == PositionEnum.Ranged).ToList();
                }

                if (targets.Count == 0)
                {
                    targets = request.Targets.Where(x => x.Position == PositionEnum.Auxillary).ToList();
                }

                // add targets to result object
                result.FirstOption.AddRange(targets);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

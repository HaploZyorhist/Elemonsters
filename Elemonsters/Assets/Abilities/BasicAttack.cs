using System.Text;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Abilities
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
            IsActive = true;
            AbilitySlot = AbilitySlotEnum.BasicAttack;
        }

        /// <inheritdoc />
        public override async Task<ActiveAbilityResult> Activation(ActiveRequest request)
        {
            try
            {
                // cannot perform an attack against nothing
                if (request.Targets.Count == 0)
                {
                    throw new Exception("There were no targets to select from");
                }

                // create result object
                var results = new ActiveAbilityResult
                {
                    ActivatingCreature = request.MyTurn,
                    Triggers = TriggerConditionsEnum.OnHit
                };

                var me = request.Creatures.Where(x => x.CreatureID == request.MyTurn).FirstOrDefault();

                // only hits the selected target
                foreach (var t in request.Targets)
                {
                    var target = request.Creatures.Where(x => x.CreatureID == t).FirstOrDefault();

                    // create object containing data on damage to be dealt
                    var damageRequest = new DamageRequest
                    {
                        Target = target.CreatureID,
                        TriggerCondition = TriggerConditionsEnum.OnHit,
                        AttackType = AttackTypeEnum.Physical,
                        Penetration = target.Stats.Lethality
                    };

                    // damage isn't modified
                    double damageModifier = 1;

                    // basic attacks can crit
                    var rand = new Random();
                    var r = rand.Next(0, 100);

                    // check for dodge first
                    if (r < target.Stats.Dodge)
                    {
                        results.Messages.AppendLine($"<@{me.User}>'s {me.Name}'s attack has missed");
                        continue;
                    }

                    r = rand.Next(0, 100);

                    if (r < me.Stats.CritChance)
                    {
                        damageModifier = 1.5;
                        results.Messages.AppendLine(
                            $"<@{me.User}>'s {me.Name} has landed a critical hit");
                    }

                    // calculates damage after crit
                    var damageDouble = me.Stats.Strength * damageModifier;

                    damageRequest.Damage = (int)damageDouble;

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
                    Rule = TargetingRulesEnum.FrontToBack,
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

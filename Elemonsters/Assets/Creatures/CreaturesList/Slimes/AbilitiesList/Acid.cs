using System.Text;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.CreaturesList.Slimes.AbilitiesList
{
    public class Acid : Ability
    {
        public Acid()
        {
            Name = "Acid";
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
                var results = new ActiveAbilityResult();

                // only hits the selected target
                foreach (var t in request.Targets)
                {
                    var target = request.Creatures.Where(x => x.CreatureID == t).FirstOrDefault();
                    var me = request.Creatures.Where(x => x.CreatureID == request.MyTurn).FirstOrDefault();

                    // create object containing data on damage to be dealt
                    var damageRequest = new DamageRequest
                    {
                        Target = target.CreatureID,
                        TriggerCondition = TriggerConditionsEnum.OnHit
                    };

                    // physical attack, uses physical attack stats
                    damageRequest.AttackType = AttackTypeEnum.Elemental;

                    // how much defense is being penetrated
                    damageRequest.Penetration = target.Stats.Sorcery;

                    // damage isn't modified
                    double damageModifier = .25;

                    //TODO add new acid status effect

                    // calculates damage after modifiers
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
    }
}

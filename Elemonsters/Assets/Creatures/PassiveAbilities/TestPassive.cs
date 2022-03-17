using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.PassiveAbilities
{
    /// <summary>
    /// this is a test passive for adding magic damage to an attack
    /// </summary>
    public class TestPassive : PassiveAbility
    {
        /// <inheritdoc />
        public override async Task<PassiveResult> Passive(PassiveRequest request)
        {
            try
            {
                // create new result object
                var result = new PassiveResult();

                // start new damage request to pass out to result
                var damageRequest = new DamageRequest
                {
                    ActiveCreature = request.MyTurn.CreatureID,
                    Target = request.Target.CreatureID
                };

                // what kind of attack is being given
                damageRequest.AttackType = AttackTypeEnum.Magic;

                // how much penetration is in the attack
                damageRequest.Penetration = request.MyTurn.Stats.Sorcery;

                // how much the damage stat is modified by
                double damageModifier = .35 + (request.AbilityLevel * .05);

                // calculate damage delt
                var damageDealt = request.MyTurn.Stats.Aura * damageModifier;

                // pass it to damage request
                damageRequest.Damage = (int)damageDealt;

                // add the damage request to the return object
                result.DamageRequests.Add(damageRequest);

                return result;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

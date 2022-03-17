using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.ActiveAbilities
{
    /// <summary>
    /// Active ability for testing shields
    /// </summary>
    public class GenerateShieldAbility : ActiveAbility
    {
        /// <inheritdoc />
        public override async Task<ActiveResult> Activation(ActiveRequest request)
        {
            try
            {
                // create return object
                var result = new ActiveResult();

                // create list of statuses to be returned
                var newStatus = new StatusRequest
                {
                    Target = request.MyTurn,
                };

                // first status to be added to creature
                var effect1 = new StatusEffect
                {
                    Name = "Test Shield",
                    Type = EffectTypes.GeneralShield,
                    Value = 1000,
                    Duration = 3,
                    Add = AddStatusEnum.Individual,
                    Remove = RemoveStatusEnum.All,
                };

                newStatus.SB.AppendLine(
                    $"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has gained {effect1.Value} {effect1.Type} for {effect1.Duration} turns");

                newStatus.Statuses.Add(effect1);

                // second effect to be added to creature
                var effect2 = new StatusEffect
                {
                    Name = "Test Physical Shield",
                    Type = EffectTypes.PhysicalShield,
                    Value = 500,
                    Duration = 2,
                    Add = AddStatusEnum.Individual,
                    Remove = RemoveStatusEnum.All,
                };

                newStatus.SB.AppendLine(
                    $"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has gained {effect2.Value} {effect2.Type} for {effect2.Duration} turns");

                newStatus.Statuses.Add(effect2);

                // third effect to be added to creature
                var effect3 = new StatusEffect
                {
                    Name = "Test Elemental Shield",
                    Type = EffectTypes.ElementalShield,
                    Value = 250,
                    Duration = 1,
                    Add = AddStatusEnum.Individual,
                    Remove = RemoveStatusEnum.All,
                };

                newStatus.SB.AppendLine(
                    $"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has gained {effect3.Value} {effect3.Type} for {effect3.Duration} turns");

                // add effects to list
                newStatus.Statuses.Add(effect3);

                // add status request to return object
                result.StatusRequests.Add(newStatus);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override async Task<GetTargetsResult> GetTargetOptions()
        {
            try
            {
                var result = new GetTargetsResult();
                result.TotalTargets = 0;

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

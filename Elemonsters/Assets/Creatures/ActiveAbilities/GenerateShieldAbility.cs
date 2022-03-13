using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.ActiveAbilities
{
    public class GenerateShieldAbility : ActiveAbility
    {
        public override async Task<ActiveResults> Activation(ActiveRequest request)
        {
            var effects = new List<StatusEffect>();

            var sb = new StringBuilder();

            var effect1 = new StatusEffect
            {
                Name = "Test Shield",
                Type = EffectTypes.GeneralShield,
                Value = 1000,
                Duration = 3
            };

            sb.AppendLine(
                $"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has gained {effect1.Value} {effect1.Type}");

            effects.Add(effect1);

            var effect2 = new StatusEffect
            {
                Name = "Test Physical Shield",
                Type = EffectTypes.PhysicalShield,
                Value = 500,
                Duration = 2
            };

            sb.AppendLine(
                $"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has gained {effect2.Value} {effect2.Type}");

            effects.Add(effect2);

            var effect3 = new StatusEffect
            {
                Name = "Test Elemental Shield",
                Type = EffectTypes.ElementalShield,
                Value = 250,
                Duration = 1
            };

            sb.AppendLine(
                $"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has gained {effect3.Value} {effect3.Type}");

            effects.Add(effect3);

            request.MyTurn.Statuses.AddRange(effects);

            var container = request.Container;

            var creature = request.Container.Creatures.Where(x => x.CreatureID == request.MyTurn.CreatureID)
                .FirstOrDefault();

            container.Creatures.Remove(creature);

            container.Creatures.Add(request.MyTurn);

            var damageResults = new List<DamageResults>();

            var damageResult = new DamageResults
            {
                AttackType = AttackTypeEnum.None,
                Damage = 0,
                Target = request.MyTurn,
                SB = sb,
                Trigger = TriggerConditions.None
            };

            damageResults.Add(damageResult);

            var result = new ActiveResults
            {
                Container = container,
                DamageResults = damageResults,
            };

            return result;
        }
    }
}

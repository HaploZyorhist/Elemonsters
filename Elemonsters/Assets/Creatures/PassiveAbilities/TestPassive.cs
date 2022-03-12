using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.PassiveAbilities
{
    public class TestPassive : PassiveAbility
    {
        public override async Task<PassiveResults> Passive(PassiveRequest request)
        {
            var targets = request.Targets;

            var result = new PassiveResults();

            foreach (var target in targets)
            {
                var damageRequest = new DamageRequest();

                var elementalBonus = new ElementalRequest
                {
                    AttackerElements = request.MyTurn.Elements,
                    DefenderElements = target.Elements
                };

                elementalBonus.AttackType = AttackTypeEnum.Magic;
                damageRequest.AttackType = AttackTypeEnum.Magic;
                damageRequest.Damage = request.MyTurn.Stats.Spirit;
                damageRequest.Defense = target.Stats.Aura;
                damageRequest.Penetration = target.Stats.Sorcery;
                damageRequest.DamageModifier = .35;
                damageRequest.ElementalRequest = elementalBonus;

                var damageDelt = await request.Container.DamageFactory.CalculateDamage(damageRequest) *
                                 await request.Container.DamageFactory.CheckElementalBonus(damageRequest.ElementalRequest);

                var roundedDamage = (int) damageDelt;

                var damageResult = new DamageResults
                {
                    AttackType = damageRequest.AttackType,
                    Damage = roundedDamage,
                    Target = target,
                    SB= damageRequest.SB
                };

                result.DamageResults.Add(damageResult);
            }

            return result;
        }
    }
}

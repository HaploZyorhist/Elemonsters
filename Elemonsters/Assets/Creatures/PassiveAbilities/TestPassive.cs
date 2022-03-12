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
        public override async Task<List<CombatResults>> Passive(BattleContainer battleContainer, CreatureBase myTurn)
        {
            var target = battleContainer.Creatures.Where(x => x.User != myTurn.User).FirstOrDefault();

            var request = new DamageRequest();

            var elementalBonus = new ElementalRequest
            {
                AttackerElements = myTurn.Elements,
                DefenderElements = target.Elements
            };

            elementalBonus.AttackType = AttackTypeEnum.Magic;
            request.AttackType = AttackTypeEnum.Magic;
            request.Damage = myTurn.Stats.Spirit;
            request.Defense = target.Stats.Aura;
            request.Penetration = target.Stats.Sorcery;
            request.DamageModifier = .35;

            var damageDelt = await battleContainer.DamageFactory.CalculateDamage(request) * await battleContainer.DamageFactory.CheckElementalBonus(elementalBonus);
            var currentHealth = target.Stats.Health;

            var roundedDamage = (int)damageDelt;

            target.Stats.Health -= roundedDamage;

            battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has dealt bonus damage to the computer's {target.Name} for {roundedDamage} {request.AttackType} damage, reducing their health from {currentHealth} to {target.Stats.Health}");

            return request;
        }
    }
}

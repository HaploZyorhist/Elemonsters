using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Factories;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.ActiveAbilities
{
    public class BasicAttackAbility : ActiveAbility
    {
        public override async Task<List<CombatResults>> Activation(BattleContainer battleContainer, CreatureBase myTurn)
        {
            try
            {
                // do targeting

                var target = battleContainer.Creatures.Where(x => x.User != myTurn.User).FirstOrDefault();

                var request = new DamageRequest();

                var elementalBonus = new ElementalRequest
                {
                    AttackerElements = myTurn.Elements,
                    DefenderElements = target.Elements
                };

                if (myTurn.CreatureID == 0)
                {
                    elementalBonus.AttackType = AttackTypeEnum.Magic;
                    request.AttackType = AttackTypeEnum.Magic;
                    request.Damage = myTurn.Stats.Spirit;
                    request.Defense = target.Stats.Aura;
                    request.Penetration = target.Stats.Sorcery;
                    request.DamageModifier = 1;
                }
                else
                {
                    elementalBonus.AttackType = AttackTypeEnum.Physical;
                    request.AttackType = AttackTypeEnum.Physical;
                    request.Damage = myTurn.Stats.Strength;
                    request.Defense = target.Stats.Defense;
                    request.Penetration = target.Stats.Lethality;
                    request.DamageModifier = 1;
                }

                var rand = new Random();
                var r = rand.Next(0, 100);

                var damageDelt = await battleContainer.DamageFactory.CalculateDamage(request) * await battleContainer.DamageFactory.CheckElementalBonus(elementalBonus);
                var currentHealth = target.Stats.Health;

                if (r < myTurn.Stats.CritChance)
                {
                    damageDelt *= myTurn.Stats.CritModifier;
                    battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has landed a critical hit");
                }

                var roundedDamage = (int)damageDelt;

                target.Stats.Health -= roundedDamage;

                battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has attacked the computer's {target.Name} for {roundedDamage} {request.AttackType} damage, reducing their health from {currentHealth} to {target.Stats.Health}");

                var passiveActivation = myTurn.Abilities
                    .Where(x => string.Equals(x.PassiveAbility?.ActivationCondition, "this", StringComparison.OrdinalIgnoreCase))?
                    .ToList();

                if (passiveActivation.Count > 0)
                {
                    foreach (var passive in passiveActivation)
                    {
                        battleContainer = await passive.PassiveAbility.Passive(battleContainer, myTurn);
                    }
                }

                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

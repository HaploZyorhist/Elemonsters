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
        public override async Task<ActiveResults> Activation(ActiveRequest request)
        {
            try
            {
                var results = new ActiveResults();

                // do targeting
                var target = request.Container.Creatures.Where(x => x.User != request.MyTurn.User).FirstOrDefault();

                var damageRequest = new DamageRequest();

                var elementalBonus = new ElementalRequest
                {
                    AttackerElements = request.MyTurn.Elements,
                    DefenderElements = target.Elements
                };

                elementalBonus.AttackType = AttackTypeEnum.Physical;
                damageRequest.AttackType = AttackTypeEnum.Physical;
                damageRequest.Damage = request.MyTurn.Stats.Strength;
                damageRequest.Defense = target.Stats.Defense;
                damageRequest.Penetration = target.Stats.Lethality;

                damageRequest.DamageModifier = 1;
                damageRequest.ElementalRequest = elementalBonus;

                var rand = new Random();
                var r = rand.Next(0, 100);

                if (r < request.MyTurn.Stats.CritChance)
                {
                    damageRequest.DamageModifier *= 1.5;
                    damageRequest.SB.AppendLine($"<@{request.MyTurn.User}>'s {request.MyTurn.Name} has landed a critical hit");
                }

                var damageDelt = await request.Container.DamageFactory.CalculateDamage(damageRequest) * await request.Container.DamageFactory.CheckElementalBonus(damageRequest.ElementalRequest);

                var roundedDamage = (int)damageDelt;

                var result = new DamageResults
                {
                    AttackType = damageRequest.AttackType,
                    Damage = roundedDamage,
                    Target = target,
                    SB = damageRequest.SB,
                    Trigger = TriggerConditions.OnHit
                };

                results.Container = request.Container;
                results.DamageResults.Add(result);

                return results;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

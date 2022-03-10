using Discord.Commands;
using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services
{
    /// <summary>
    /// class for handling battle related functions
    /// </summary>
    public class BattleService : IBattleService
    {
        /// <inheritdoc />
        public async Task BeginBattle(ICommandContext context, List<CreatureBase> player1Party, List<CreatureBase> player2Party)
        {
            DamageFactory _damageFactory = new DamageFactory();

            try
            {
                var sb = new StringBuilder();

                var attacker = player1Party[0];

                var defender = player2Party[0];

                // deal true damage

                var currentHealth = defender.Stats.Health;

                var request = new DamageRequest
                {
                    AttackType = AttackTypeEnum.True,
                    Damage = attacker.Stats.Strength,
                    Defense = defender.Stats.Defense,
                    Penetration = attacker.Stats.Lethality,
                    DamageModifier = 1
                };

                var elementalBonus = new ElementalRequest
                {
                    AttackType = AttackTypeEnum.True,
                    AttackerElements = attacker.Elements,
                    DefenderElements = defender.Elements
                };

                var damageDelt = await _damageFactory.CalculateDamage(request) * await _damageFactory.CheckElementalBonus(elementalBonus);

                defender.Stats.Health -= (int)damageDelt;

                sb.AppendLine($"<@{attacker.User}>'s {attacker.Name} has attacked the computer's {defender.Name} for {(int)damageDelt} true damage, reducing their health from {currentHealth} to {defender.Stats.Health}");

                // deal physical damage

                currentHealth = defender.Stats.Health;

                request.AttackType = AttackTypeEnum.Physical;
                elementalBonus.AttackType = AttackTypeEnum.Physical;

                damageDelt = await _damageFactory.CalculateDamage(request) * await _damageFactory.CheckElementalBonus(elementalBonus);

                defender.Stats.Health -= (int)damageDelt;

                sb.AppendLine($"<@{attacker.User}>'s {attacker.Name} has attacked the computer's {defender.Name} for {(int)damageDelt} physical damage, reducing their health from {currentHealth} to {defender.Stats.Health}");

                // deal magic damage

                currentHealth = defender.Stats.Health;

                request.AttackType = AttackTypeEnum.Magic;
                request.Damage = attacker.Stats.Spirit;
                request.Defense = defender.Stats.Aura;
                request.Penetration = attacker.Stats.Sorcery;
                elementalBonus.AttackType = AttackTypeEnum.Magic;

                damageDelt = await _damageFactory.CalculateDamage(request) * await _damageFactory.CheckElementalBonus(elementalBonus);

                defender.Stats.Health -= (int)damageDelt;

                sb.AppendLine($"<@{attacker.User}>'s {attacker.Name} has attacked the computer's {defender.Name} for {(int)damageDelt} magic damage, reducing their health from {currentHealth} to {defender.Stats.Health}");

                await context.Channel.SendMessageAsync(sb.ToString());
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}

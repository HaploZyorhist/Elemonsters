using Discord.Commands;
using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models;
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

                var damageDelt = await _damageFactory.CalculateDamage(AttackTypeEnum.True, attacker, defender);

                defender.Stats.Health -= damageDelt;

                sb.AppendLine($"<@{attacker.User}>'s {attacker.Name} has attacked the computer's {defender.Name} for {damageDelt} true damage, reducing their health from {currentHealth} to {defender.Stats.Health}");

                // deal physical damage

                currentHealth = defender.Stats.Health;

                damageDelt = await _damageFactory.CalculateDamage(AttackTypeEnum.Physical, attacker, defender);

                defender.Stats.Health -= damageDelt;

                sb.AppendLine($"<@{attacker.User}>'s {attacker.Name} has attacked the computer's {defender.Name} for {damageDelt} physical damage, reducing their health from {currentHealth} to {defender.Stats.Health}");

                // deal magic damage

                currentHealth = defender.Stats.Health;

                damageDelt = await _damageFactory.CalculateDamage(AttackTypeEnum.Magic, attacker, defender);

                defender.Stats.Health -= damageDelt;

                sb.AppendLine($"<@{attacker.User}>'s {attacker.Name} has attacked the computer's {defender.Name} for {damageDelt} magic damage, reducing their health from {currentHealth} to {defender.Stats.Health}");

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

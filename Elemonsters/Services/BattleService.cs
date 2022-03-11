using Discord;
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
        private readonly IPartyService _partyService;
        private readonly DamageFactory _damageFactory;

        public BattleService(IPartyService partyService,
                             DamageFactory damageFactory)
        {
            _partyService = partyService;
            _damageFactory = damageFactory;
        }

        /// <inheritdoc />
        public async Task BeginBattle(BattleContainer battleContainer)
        {
            try
            {
                if (battleContainer.Players.Count == 1)
                {
                    var bot = await battleContainer.Context.Client.GetUserAsync(Environment.GetEnvironmentVariable("BotName"), Environment.GetEnvironmentVariable("BotDiscriminator"));
                    battleContainer.Players.Add(bot);
                }

                var sb = new StringBuilder();

                var creatures = new List<CreatureBase>();

                foreach(var player in battleContainer.Players)
                {
                    var party = await _partyService.GetParty(player.Id);
                    creatures.AddRange(party);
                    sb.AppendLine($"{player.Mention}, you have the monster {creatures.Where(x => x.User == player.Id).FirstOrDefault().Name} in your party");
                }

                battleContainer.Creatures.AddRange(creatures);

                await battleContainer.Context.Channel.SendMessageAsync(sb.ToString());
                sb.Clear();

                // loop while a player's team has 1 member alive
                var aliveCreatures = battleContainer.Creatures.Where(x => x.Stats.Health > 0).ToList();
                var teamAAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[0].Id).ToList();
                var teamBAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[1].Id).ToList();

                while(teamAAliveMembers.Count > 0 && teamBAliveMembers.Count > 0)
                {
                    
                    var creaturesToTurn = aliveCreatures.Where(x => x.ActionPoints >= 100).OrderBy(x => x.ActionPoints).ThenBy(x => x.Stats.Speed).ToList();

                    foreach(var myTurn in creaturesToTurn)
                    {
                        battleContainer = await PerformTurn(battleContainer, myTurn);

                        myTurn.ActionPoints -= 100;
                        sb.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has taken a turn and reduced their action points by 100");
                    }

                    foreach(var alive in aliveCreatures)
                    {
                        await alive.Tick();
                    }

                    aliveCreatures = battleContainer.Creatures.Where(x => x.Stats.Health > 0).ToList();
                    teamAAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[0].Id).ToList();
                    teamBAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[1].Id).ToList();

                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        await battleContainer.Context.Channel.SendMessageAsync(sb.ToString());
                        sb.Clear();
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<BattleContainer> PerformTurn(BattleContainer battleContainer, CreatureBase myTurn)
        {
            try
            {
                var target = battleContainer.Creatures.Where(x => x.CreatureID != myTurn.CreatureID).FirstOrDefault();

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

                var damageDelt = await _damageFactory.CalculateDamage(request) * await _damageFactory.CheckElementalBonus(elementalBonus);
                var currentHealth = target.Stats.Health;

                var roundedDamage = (int)damageDelt;

                target.Stats.Health -= roundedDamage;

                var sb = new StringBuilder();
                sb.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has attacked the computer's {target.Name} for {roundedDamage} {request.AttackType} damage, reducing their health from {currentHealth} to {target.Stats.Health}");

                await battleContainer.Context.Channel.SendMessageAsync(sb.ToString());

                return battleContainer;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}

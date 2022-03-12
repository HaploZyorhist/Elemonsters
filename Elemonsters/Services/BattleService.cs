using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;
using System.Text;

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

                var creatures = new List<CreatureBase>();

                foreach(var player in battleContainer.Players)
                {
                    var party = await _partyService.GetParty(player.Id);
                    creatures.AddRange(party);
                    battleContainer.SB.AppendLine($"{player.Mention}, you have the monster {creatures.Where(x => x.User == player.Id).FirstOrDefault().Name} in your party");
                }

                battleContainer.Creatures.AddRange(creatures);

                await battleContainer.Context.Channel.SendMessageAsync(battleContainer.SB.ToString());

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
                        battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has taken a turn and reduced their action points by 100");

                        aliveCreatures = battleContainer.Creatures.Where(x => x.Stats.Health > 0).ToList();
                        teamAAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[0].Id).ToList();
                        teamBAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[1].Id).ToList();

                        if (teamAAliveMembers.Count !> 0 || teamBAliveMembers.Count !> 0)
                        {
                            var sb = teamAAliveMembers.Count > 0
                                ? $"<@{teamAAliveMembers[0].User}> has won the battle"
                                : $"<@{teamBAliveMembers[0].User}> has won the battle";
                            battleContainer.SB.AppendLine(sb);
                            break;
                        }
                    }

                    foreach(var alive in aliveCreatures)
                    {
                        await alive.Tick();
                    }

                    if (!string.IsNullOrEmpty(battleContainer.SB.ToString()))
                    {
                        await battleContainer.Context.Channel.SendMessageAsync(battleContainer.SB.ToString());
                        battleContainer.SB.Clear();
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// method for giving a creature their turn
        /// </summary>
        /// <param name="battleContainer">the state of the battle</param>
        /// <param name="myTurn">the creature who is taking their turn</param>
        /// <returns>updated map state</returns>
        public async Task<BattleContainer> PerformTurn(BattleContainer battleContainer, CreatureBase myTurn)
        {
            try
            {
                var energyGained = await myTurn.Gain(0, 1);

                battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has gained {energyGained} energy, bringing them to {myTurn.Stats.Energy}");

                battleContainer = await myTurn.Abilities
                    .Where(x => string.Equals(x.Name, "Basic Attack", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()
                    .ActiveAbility
                    .Activation(battleContainer, myTurn);

                return battleContainer;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}

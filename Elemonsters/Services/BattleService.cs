using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;
using System.Text;
using Microsoft.VisualBasic;

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
                battleContainer.SB.Clear();

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

                        if (teamAAliveMembers.Count == 0 || teamBAliveMembers.Count == 0)
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
                // do active ability
                // check passives
                var energyGained = await myTurn.Gain(0, 1);

                battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has gained {energyGained} energy, bringing them to {myTurn.Stats.Energy}");

                var selectedAbility = myTurn.Abilities
                    .Where(x => string.Equals(x.Name, "Basic Attack", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                
                var activeRequest = new ActiveRequest
                {
                    Container = battleContainer,
                    MyTurn = myTurn,
                    AbilityName = selectedAbility.Name,
                    AbilityLevel = selectedAbility.AbilityLevel
                };

                var activeResults = await selectedAbility.ActiveAbility.Activation(activeRequest);

                foreach (var result in activeResults.DamageResults)
                {
                    battleContainer.SB.Append(result.SB.ToString());
                }

                var passiveActivations = myTurn.Abilities
                    .Where(x => x.PassiveAbility != null && 
                                       string.Equals(x.PassiveAbility.ActivationCondition, "this", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var passivesKicked = new List<PassiveResults>();

                foreach (var passive in passiveActivations)
                {
                    var passiveRequest = new PassiveRequest
                    {
                        Container = battleContainer,
                        MyTurn = myTurn,
                        Targets = activeResults.DamageResults.Select(x => x.Target).ToList(),
                        AbilityName = passive.Name,
                        AbilityLevel = passive.AbilityLevel
                    };

                    var passiveKick = await passive.PassiveAbility.Passive(passiveRequest);

                    passivesKicked.Add(passiveKick);
                }

                var targetList = activeResults.DamageResults.Select(x => x.Target).Distinct();

                foreach (var target in targetList)
                {
                    int totalDamage = 0;

                    var physicalDamage = activeResults.DamageResults
                        .Where(x => x.Target == target)
                        .Select(x => x.Damage)
                        .Sum();

                    var magicDamage = passivesKicked
                        .Select(x => x.DamageResults
                            .Where(x => x.Target == target)
                            .Select(x => x.Damage)
                            .Sum())
                        .Sum();

                    var creature = battleContainer.Creatures.Where(x => x.CreatureID == target.CreatureID).FirstOrDefault();

                    var currentHealth = creature.Stats.Health;

                    totalDamage = physicalDamage + magicDamage;
                    creature.Stats.Health -= totalDamage;

                    battleContainer.SB.AppendLine(
                        $"<@{creature.User}>'s {creature.Name} has taken {physicalDamage} physical damage");

                    battleContainer.SB.AppendLine(
                        $"<@{creature.User}>'s {creature.Name} has taken {magicDamage} magic damage");

                    battleContainer.SB.AppendLine(
                        $"<@{creature.User}>'s {creature.Name} has taken {currentHealth - creature.Stats.Health} total damage, leaving them at {creature.Stats.Health} health");
                }

                return battleContainer;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}

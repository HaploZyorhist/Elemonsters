using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;
using System.Text;
using Discord.Commands;
using Elemonsters.Models.Chat;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Microsoft.EntityFrameworkCore.Query;

namespace Elemonsters.Services
{
    /// <summary>
    /// class for handling battle related functions
    /// </summary>
    public class BattleService : IBattleService
    {
        private readonly IPartyService _partyService;
        private readonly DamageFactory _damageFactory;
        private readonly IChatService _chatService;

        public ICommandContext Context;
        public BattleContainer Container;

        public BattleService(IPartyService partyService,
                             DamageFactory damageFactory,
                             IChatService chatService)
        {
            _partyService = partyService;
            _damageFactory = damageFactory;
            _chatService = chatService;
        }

        /// <inheritdoc />
        public async Task BeginBattle(ICommandContext context, BattleContainer battleContainer)
        {
            Context = context;
            Container = battleContainer;

            try
            {
                if (battleContainer.Players.Count == 1)
                {
                    var bot = await Context.Client.GetUserAsync(Environment.GetEnvironmentVariable("BotName"), Environment.GetEnvironmentVariable("BotDiscriminator"));
                    battleContainer.Players.Add(bot);
                }

                var creatures = new List<CreatureBase>();

                foreach (var player in battleContainer.Players)
                {
                    var party = await _partyService.GetParty(player.Id);
                    creatures.AddRange(party);
                    battleContainer.SB.AppendLine($"{player.Mention}, you have the monster {creatures.Where(x => x.User == player.Id).FirstOrDefault().Name} in your party");
                }

                battleContainer.Creatures.AddRange(creatures);

                await Context.Channel.SendMessageAsync(battleContainer.SB.ToString());
                battleContainer.SB.Clear();

                // loop while both players have at least 1 alive team member
                var winner = await StartBattleLoop(battleContainer);

                return;
            }
            catch (Exception ex)
            {
                // await battleContainer.Context.Channel.SendMessageAsync(ex.Message);

                return;
            }
        }

        /// <summary>
        /// loops until someone has won the battle
        /// </summary>
        /// <param name="battleContainer">object that contains the battle information</param>
        /// <returns>string of the winner</returns>
        private async Task<string> StartBattleLoop(BattleContainer battleContainer)
        {
            try
            {
                // who the winner is
                string winner = "";

                // creatures that are not dead at the moment
                var aliveCreatures = battleContainer.Creatures.Where(x => x.Stats.Health > 0).ToList();
                var teamAAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[0].Id).ToList();
                var teamBAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[1].Id).ToList();

                // the loop that will continue until 1 player has no alive members
                while (teamAAliveMembers.Count > 0 && teamBAliveMembers.Count > 0)
                {
                    // gets creatures who need to take a turn
                    var creaturesToTurn = aliveCreatures.Where(x => x.ActionPoints >= 100)
                        .OrderBy(x => x.ActionPoints)
                        .ThenBy(x => x.Stats.Speed)
                        .ThenByDescending(x => x.Level)
                        .ThenBy(x => x.CreatureID)
                        .ToList();

                    // go through each creature who needs to take a turn
                    foreach (var myTurn in creaturesToTurn)
                    {
                        // perform that creatures turn
                        battleContainer = await PerformTurn(battleContainer, myTurn);

                        if (battleContainer == null)
                        {
                            throw new Exception("PerformTurn has returned null");
                        }

                        // reduce their turn points by 100 for taking a turn
                        myTurn.ActionPoints -= 100;
                        battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has taken a turn and reduced their action points by 100");

                        // recheck alive creatures
                        aliveCreatures = battleContainer.Creatures.Where(x => x.Stats.Health > 0).ToList();
                        teamAAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[0].Id).ToList();
                        teamBAliveMembers = aliveCreatures.Where(x => x.User == battleContainer.Players[1].Id).ToList();

                        // check win condition
                        if (teamAAliveMembers.Count == 0 || teamBAliveMembers.Count == 0)
                        {
                            var sb = teamAAliveMembers.Count > 0
                                ? $"<@{teamAAliveMembers[0].User}> has won the battle"
                                : $"<@{teamBAliveMembers[0].User}> has won the battle";
                            battleContainer.SB.AppendLine(sb);
                            winner = $"<@{teamAAliveMembers[0].User}>";
                            break;
                        }

                        // pass messages out
                        if (!string.IsNullOrEmpty(battleContainer.SB.ToString()))
                        {
                            await Context.Channel.SendMessageAsync(battleContainer.SB.ToString());
                            battleContainer.SB.Clear();
                        }
                    }

                    // tick up the turn counter for all creatures
                    foreach (var alive in aliveCreatures)
                    {
                        await alive.Tick();
                    }
                }

                return winner;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// method for giving a creature their turn
        /// </summary>
        /// <param name="battleContainer">the state of the battle</param>
        /// <param name="myTurn">the creature who is taking their turn</param>
        /// <returns>updated map state</returns>
        private async Task<BattleContainer> PerformTurn(BattleContainer battleContainer, CreatureBase myTurn)
        {
            try
            {
                // step 1, gain energy
                var energyGained = await myTurn.Gain(0, 1);

                battleContainer.SB.AppendLine($"<@{myTurn.User}>'s {myTurn.Name} has gained {energyGained} energy, bringing them to {myTurn.Stats.Energy}");

                // step 2 countdown statuses
                var statusRemoval = await CountdownStatuses(myTurn);

                battleContainer.SB.Append(statusRemoval.SB.ToString());

                // loop through this until an ability can successfully be performed
                ActiveResult activeResults = null;

                while (activeResults == null)
                {
                    // step 3 select and ability to use
                    var selectedAbility = await SelectAbility(myTurn);

                    if (selectedAbility == null)
                    {
                        throw new Exception($"<@{myTurn.User}> failed to select an ability properly");
                    }

                    // step 4 get target options
                    var targetsRequest = new GetTargetsRequest
                    {
                        MyTurn = myTurn,
                        Targets = battleContainer.Creatures
                    };
                    var targetOptions = await selectedAbility.ActiveAbility.GetTargetOptions(targetsRequest);

                    // step 5 get player response for targets
                    var targets = new List<CreatureBase>();

                    if (targetOptions.TotalTargets > 0)
                    {
                        targets = await SelectTargets(targetOptions, myTurn);
                    }

                    // step 6 get results of the activated ability
                    var activeRequest = new ActiveRequest
                    {
                        MyTurn = myTurn,
                        AbilityName = selectedAbility.Name,
                        AbilityLevel = selectedAbility.AbilityLevel,
                        Targets = targets
                    };

                    activeResults = await selectedAbility.ActiveAbility.Activation(activeRequest);
                }

                // step 7 do things from the abilities
                foreach (var result in activeResults.DamageRequests)
                {
                    battleContainer.SB.Append(result.SB.ToString());
                }

                //TODO come back and finish the passive activation thingy

                //// step 8 get passives that were kicked
                //var passivesToActivate = new List<Ability>();

                //foreach (var dReq in activeResults.DamageRequests)
                //{
                //    var passivesActivated = await GetTriggeredPassives(dReq);
                //    dReq.
                //    passivesToActivate.AddRange(passivesActivated);
                //}

                //// step 9 get the results of all passives kicked
                //var passiveResults = new List<PassiveResult>();

                //foreach (var ability in passivesToActivate)
                //{
                //    var passiveRequest = new PassiveRequest
                //    {
                //        Targets = ??,
                //        AbilityName = ability.Name,
                //        AbilityLevel = ability.AbilityLevel,
                //        MyTurn = ??
                //    }; 

                //    var passiveResult = await ability.PassiveAbility.Passive(passiveRequest);

                //    passiveResults.Add(passiveResult);
                //}

                // step 10 do all damage requests
                var damageRequests = new List<DamageRequest>();
                damageRequests.AddRange(activeResults.DamageRequests);
                //passiveResults.ForEach(x => damageRequests.AddRange(x.DamageRequests));

                var damageResults = await HandleDamage(damageRequests);

                var targetList = damageRequests.Select(x => x.Target).Distinct().ToList();

                return battleContainer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// method for counting down status effects
        /// </summary>
        /// <param name="myTurn">creature who is having statuses counted down</param>
        /// <returns>object with the results of the status countdown</returns>
        private async Task<StatusCountdownReturn> CountdownStatuses(CreatureBase myTurn)
        {
            try
            {
                StatusCountdownReturn countdownReturn = new StatusCountdownReturn();

                foreach (var status in myTurn.Statuses)
                {
                    status.Duration -= 1;
                }

                var timedoutStatus = myTurn.Statuses.Where(x => x.Duration < 1).ToList();

                foreach (var status in timedoutStatus)
                {
                    countdownReturn.SB.AppendLine($"{status.Name} has timed out");
                }

                //TODO get removal type and perform the removal properly
                myTurn.Statuses.RemoveAll(x => x.Duration < 1);

                countdownReturn.MyTurn = myTurn;

                return countdownReturn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// method for selecting abilities
        /// </summary>
        /// <param name="battleContainer">container for the battle details</param>
        /// <param name="myTurn">creature who's turn it is</param>
        /// <returns>an ability that is available to the player</returns>
        private async Task<Ability> SelectAbility(CreatureBase myTurn)
        {
            try
            {
                var sb = new StringBuilder();

                Ability selectedAbility = null;

                if (myTurn.User == 947509644706869269)
                {
                    //TODO move this to AI Service

                    selectedAbility = myTurn.Abilities
                        .Where(x => string.Equals(x.Name, "Basic Attack", StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();
                }
                else
                {
                    while (selectedAbility == null)
                    {
                        var abilityOptions = myTurn.Abilities
                                .Where(x => x.ActiveAbility != null)
                                .ToList();

                        sb.AppendLine(
                            $"<@{myTurn.User}> please select an ability.");

                        for (int i = 0; i < abilityOptions.Count; i++)
                        {
                            sb.AppendLine($"{i + 1}: {abilityOptions[i].Name}");
                        }

                        var playerResponseRequest = new PlayerResponseRequest
                        {
                            User = myTurn.User,
                            Message = sb.ToString(),
                        };

                        var response = await _chatService.GetPlayerResponse(playerResponseRequest);

                        sb.Clear();

                        if (response == null)
                        {
                            throw new Exception($"<@{myTurn.User}> has left the battle");
                        }
                        else if (!int.TryParse(response, out int intResponse) ||
                                 intResponse > abilityOptions.Count ||
                                 intResponse < 1)
                        {
                            sb.AppendLine($"<@{myTurn.User}> please enter a valid selection");
                        }
                        else
                        {
                            selectedAbility = abilityOptions[intResponse - 1];
                        }
                    }
                }

                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    await Context.Channel.SendMessageAsync(sb.ToString());
                }

                return selectedAbility;
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync(ex.Message);

                return null;
            }
        }

        /// <summary>
        /// method for having player select targets for their abilities
        /// </summary>
        /// <param name="targetOptions">the options given to the player</param>
        /// <param name="myTurn">creature who is taking the turn</param>
        /// <returns>list of target options the player has selected</returns>
        private async Task<List<CreatureBase>> SelectTargets(GetTargetsResult targetOptions, CreatureBase myTurn)
        {
            try
            {
                var playerResponse = new PlayerResponseRequest
                {
                    Context = Context,
                    User = myTurn.User,
                };

                var sb = new StringBuilder();

                var targets = new List<CreatureBase>();
                var firstSelection = new List<CreatureBase>();
                var secondSelection = new List<CreatureBase>();
                var thirdSelection = new List<CreatureBase>();

                while (firstSelection.Count < targetOptions.FirstOptionTargets &&
                       targetOptions.FirstOption.Count > 0)
                {
                    sb.AppendLine($"<@{myTurn.User}> please select an enemy to attack");

                    for (var i = 0; i < targetOptions.FirstOption.Count; i++)
                    {
                        sb.AppendLine($"{i + 1}: {targetOptions.FirstOption[i].Name}");
                    }

                    playerResponse.Message = sb.ToString();

                    var chosenTargets = await _chatService.GetPlayerResponse(playerResponse);

                    sb.Clear();

                    if (chosenTargets == null)
                    {
                        throw new Exception($"<@{myTurn.User}> has left the battle");
                    }
                    else if (!int.TryParse(chosenTargets, out int intResponse) ||
                             intResponse > targetOptions.FirstOption.Count ||
                             intResponse < 1)
                    {
                        sb.AppendLine($"<@{myTurn.User}> please enter a valid selection");
                    }
                    else
                    {
                        firstSelection.Add(targetOptions.FirstOption[intResponse - 1]);
                        targetOptions.FirstOption.Remove(targetOptions.FirstOption[intResponse - 1]);
                    }
                }

                targets.AddRange(firstSelection);

                while (secondSelection.Count < targetOptions.SecondOptionTargets &&
                       targetOptions.SecondOption.Count > 0)
                {
                    sb.AppendLine($"<@{myTurn.User}> please select an enemy to attack");

                    for (var i = 0; i < targetOptions.SecondOption.Count; i++)
                    {
                        sb.AppendLine($"{i + 1}: {targetOptions.SecondOption[i].Name}");
                    }

                    playerResponse.Message = sb.ToString();

                    var chosenTargets = await _chatService.GetPlayerResponse(playerResponse);

                    sb.Clear();

                    if (chosenTargets == null)
                    {
                        throw new Exception($"<@{myTurn.User}> has left the battle");
                    }
                    else if (!int.TryParse(chosenTargets, out int intResponse) ||
                             intResponse > targetOptions.SecondOption.Count ||
                             intResponse < 1)
                    {
                        sb.AppendLine($"<@{myTurn.User}> please enter a valid selection");
                    }
                    else
                    {
                        secondSelection.Add(targetOptions.SecondOption[intResponse - 1]);
                        targetOptions.SecondOption.Remove(targetOptions.SecondOption[intResponse - 1]);
                    }
                }

                targets.AddRange(secondSelection);

                while (thirdSelection.Count < targetOptions.ThirdOptionTargets &&
                       targetOptions.ThirdOption.Count > 0)
                {
                    sb.AppendLine($"<@{myTurn.User}> please select an enemy to attack");

                    for (var i = 0; i < targetOptions.ThirdOption.Count; i++)
                    {
                        sb.AppendLine($"{i + 1}: {targetOptions.ThirdOption[i].Name}");
                    }

                    playerResponse.Message = sb.ToString();

                    var chosenTargets = await _chatService.GetPlayerResponse(playerResponse);

                    sb.Clear();

                    if (chosenTargets == null)
                    {
                        throw new Exception($"<@{myTurn.User}> has left the battle");
                    }
                    else if (!int.TryParse(chosenTargets, out int intResponse) ||
                             intResponse > targetOptions.ThirdOption.Count ||
                             intResponse < 1)
                    {
                        sb.AppendLine($"<@{myTurn.User}> please enter a valid selection");
                    }
                    else
                    {
                        thirdSelection.Add(targetOptions.ThirdOption[intResponse - 1]);
                        targetOptions.ThirdOption.Remove(targetOptions.ThirdOption[intResponse - 1]);
                    }
                }

                targets.AddRange(thirdSelection);

                return targets;
            }
            catch (Exception cat)
            {
                return null;
            }
        }

        /// <summary>
        /// method for triggering effects
        /// </summary>
        /// <returns>list of abilities that were triggered</returns>
        private async Task<List<Ability>> GetTriggeredPassives(DamageRequest request)
        {
            try
            {
                var triggeredAbilities = new List<Ability>();

                var triggers = request.ActiveCreature.Abilities.Where(x =>
                    (x.PassiveAbility.AllowedActivators == null ||
                     x.PassiveAbility.AllowedActivators.Contains(request.ActiveCreature.CreatureID)) &&
                    ((x.PassiveAbility.TriggerConditions == TriggerConditions.DamageDealt && request.Damage > 0) ||
                      x.PassiveAbility.TriggerConditions == request.TriggerCondition)).ToList();

                var selfTriggers = request.Target.Abilities.Where(x =>
                    (x.PassiveAbility.AllowedActivators != null ||
                     x.PassiveAbility.AllowedActivators.Contains(request.ActiveCreature.CreatureID) &&
                     x.PassiveAbility.TriggerConditions == TriggerConditions.DamageTaken)).ToList();

                triggeredAbilities.AddRange(triggers);
                triggeredAbilities.AddRange(selfTriggers);


                return triggeredAbilities;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CreatureBase> HandleDamage(List<DamageRequest> damageRequests)
        {
            try
            {
                foreach (var target in targetList)
                {
                    int totalDamage = 0;

                    var physicalDamage = activeResults.DamageRequests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.Physical)
                        .Select(x => x.Damage)
                        .Sum();

                    physicalDamage += passivesKicked
                        .Select(x => x.DamageRequests
                            .Where(x => x.Target == target &&
                                        x.AttackType == AttackTypeEnum.Physical)
                            .Select(x => x.Damage)
                            .Sum())
                        .Sum();

                    var magicDamage = activeResults.DamageRequests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.Magic)
                        .Select(x => x.Damage)
                        .Sum();

                    magicDamage += passivesKicked
                        .Select(x => x.DamageRequests
                            .Where(x => x.Target == target &&
                                        x.AttackType == AttackTypeEnum.Magic)
                            .Select(x => x.Damage)
                            .Sum())
                        .Sum();

                    var creature = battleContainer.Creatures.Where(x => x.CreatureID == target.CreatureID).FirstOrDefault();

                    var currentHealth = creature.Stats.Health;

                    var elementalShield = creature.Statuses.Where(x => x.Type == EffectTypes.ElementalShield)
                        .FirstOrDefault();

                    var physicalShield = creature.Statuses.Where(x => x.Type == EffectTypes.PhysicalShield)
                        .FirstOrDefault();

                    var generalShield = creature.Statuses.Where(x => x.Type == EffectTypes.GeneralShield)
                        .FirstOrDefault();

                    int remainingDamage;

                    if (elementalShield != null && magicDamage > 0)
                    {
                        remainingDamage = 0;

                        var currentShield = elementalShield.Value;

                        elementalShield.Value -= magicDamage;

                        if (elementalShield.Value < 0)
                        {
                            remainingDamage = -elementalShield.Value;
                            elementalShield.Value = 0;
                        }

                        battleContainer.SB.AppendLine(
                            $"<@{creature.User}>'s {creature.Name} has taken {magicDamage} elemental damage. Their elemental shield blocked {currentShield - elementalShield.Value}");

                        battleContainer.SB.AppendLine(
                            $"{creature.Name} has {elementalShield.Value} remaining elemental shield");

                        magicDamage = remainingDamage;
                    }
                    else if (magicDamage > 0)
                    {
                        battleContainer.SB.AppendLine(
                            $"<@{creature.User}>'s {creature.Name} has taken {magicDamage} elemental damage");
                    }

                    if (physicalShield != null && physicalDamage > 0)
                    {
                        remainingDamage = 0;

                        var currentShield = physicalShield.Value;

                        physicalShield.Value -= physicalDamage;

                        if (physicalShield.Value < 0)
                        {
                            remainingDamage = -physicalShield.Value;
                            physicalShield.Value = 0;
                        }

                        battleContainer.SB.AppendLine(
                            $"<@{creature.User}>'s {creature.Name} has taken {physicalDamage} physical damage. Their physical shield blocked {currentShield - physicalShield.Value}");

                        battleContainer.SB.AppendLine(
                            $"{creature.Name} has {physicalShield.Value} remaining physical shield");

                        physicalDamage = remainingDamage;
                    }
                    else if (physicalDamage > 0)
                    {
                        battleContainer.SB.AppendLine(
                            $"<@{creature.User}>'s {creature.Name} has taken {physicalDamage} physical damage");
                    }

                    totalDamage = physicalDamage + magicDamage;

                    if (generalShield != null && totalDamage > 0)
                    {
                        remainingDamage = 0;

                        var currentShield = generalShield.Value;

                        generalShield.Value -= totalDamage;

                        if (generalShield.Value < 0)
                        {
                            remainingDamage = -generalShield.Value;
                            generalShield.Value = 0;
                        }

                        battleContainer.SB.AppendLine(
                            $"<@{creature.User}>'s {creature.Name} has taken {totalDamage} remaining damage. Their general shield blocked {currentShield - generalShield.Value}");

                        battleContainer.SB.AppendLine(
                            $"{creature.Name} has {generalShield.Value} remaining general shield");

                        totalDamage = remainingDamage;
                    }
                    else if (totalDamage > 0)
                    {
                        battleContainer.SB.AppendLine(
                            $"<@{creature.User}>'s {creature.Name} has taken {totalDamage} remaining damage");

                        creature.Stats.Health -= totalDamage;

                        battleContainer.SB.AppendLine(
                            $"<@{creature.User}>'s {creature.Name} has taken {currentHealth - creature.Stats.Health} total damage, leaving them at {creature.Stats.Health} health");
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
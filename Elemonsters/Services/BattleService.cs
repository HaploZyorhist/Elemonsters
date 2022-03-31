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
using Elemonsters.Models.Factory.Requests;

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
        private readonly ITargetingService _targetingService;

        public ICommandContext _context;
        public BattleContainer _container;
        public StringBuilder _messages = new StringBuilder();

        public BattleService(IPartyService partyService,
                             DamageFactory damageFactory,
                             IChatService chatService,
                             ITargetingService targetingService)
        {
            _partyService = partyService;
            _damageFactory = damageFactory;
            _chatService = chatService;
            _targetingService = targetingService;
        }

        /// <inheritdoc />
        public async Task BeginBattle(ICommandContext context, BattleContainer battleContainer)
        {
            _context = context;
            _container = battleContainer;

            try
            {
                if (battleContainer.Players.Count == 1)
                {
                    var bot = await _context.Client.GetUserAsync(Environment.GetEnvironmentVariable("BotName"), Environment.GetEnvironmentVariable("BotDiscriminator"));
                    battleContainer.Players.Add(bot);
                }

                var creatures = new List<CreatureBase>();

                foreach (var player in battleContainer.Players)
                {
                    var party = await _partyService.GetParty(player.Id);
                    creatures.AddRange(party);
                    _messages.AppendLine($"{player.Mention}, you have the monster {creatures.Where(x => x.User == player.Id).FirstOrDefault().Name} in your party");
                }

                battleContainer.Creatures.AddRange(creatures);

                await _context.Channel.SendMessageAsync(_messages.ToString());
                _messages.Clear();

                // loop while both players have at least 1 alive team member
                var winner = await StartBattleLoop();

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
        /// <returns>string of the winner</returns>
        private async Task<string> StartBattleLoop()
        {
            try
            {
                // who the winner is
                string winner = "";

                // creatures that are not dead at the moment
                var aliveCreatures = _container.Creatures.Where(x => x.Stats.Health > 0).ToList();
                var teamAAliveMembers = aliveCreatures.Where(x => x.User == _container.Players[0].Id).ToList();
                var teamBAliveMembers = aliveCreatures.Where(x => x.User == _container.Players[1].Id).ToList();

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
                    foreach (var me in creaturesToTurn)
                    {
                        // perform that creatures turn
                        await PerformTurn(me.CreatureID);

                        // reduce their turn points by 100 for taking a turn
                        me.ActionPoints -= 100;
                        _messages.AppendLine($"<@{me.User}>'s {me.Name} has taken a turn and reduced their action points by 100");

                        // recheck alive creatures
                        aliveCreatures = _container.Creatures.Where(x => x.Stats.Health > 0).ToList();
                        teamAAliveMembers = aliveCreatures.Where(x => x.User == _container.Players[0].Id).ToList();
                        teamBAliveMembers = aliveCreatures.Where(x => x.User == _container.Players[1].Id).ToList();

                        // check win condition
                        if (teamAAliveMembers.Count == 0 || teamBAliveMembers.Count == 0)
                        {
                            var sb = teamAAliveMembers.Count > 0
                                ? $"<@{teamAAliveMembers[0].User}> has won the battle"
                                : $"<@{teamBAliveMembers[0].User}> has won the battle";
                            _messages.AppendLine(sb);
                            winner = $"<@{teamAAliveMembers[0].User}>";
                            break;
                        }

                        // pass messages out
                        if (!string.IsNullOrEmpty(_messages.ToString()))
                        {
                            await _context.Channel.SendMessageAsync(_messages.ToString());
                            _messages.Clear();
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
        /// <param name="myTurn">the creature who is taking their turn</param>
        private async Task PerformTurn(ulong myTurn)
        {
            try
            {
                // step 1 get creature taking turn
                var me = _container.Creatures.Where(x => x.CreatureID == myTurn).FirstOrDefault();

                // step 1, gain energy
                var energyGained = await me.Gain(0, 1);

                _messages.AppendLine($"<@{me.User}>'s {me.Name} has gained {energyGained} energy, bringing them to {me.Stats.Energy}");

                // step 2 countdown statuses
                await CountdownStatuses(myTurn);

                // loop through this until an ability can successfully be performed
                ActiveResult activeResults = null;

                while (activeResults == null)
                {
                    // step 3 select and ability to use
                    var selectedAbility = await SelectAbility(myTurn);

                    if (selectedAbility == null)
                    {
                        throw new Exception($"<@{me.User}> failed to select an ability properly");
                    }

                    // step 4 get target options
                    var targetRules = await selectedAbility.GetTargetOptions();

                    var targets = new List<ulong>();

                    if (targetRules.Rule != TargetingRulesEnum.NoTarget)
                    {
                        var targetingRequest = new GetTargetsRequest
                        {
                            Rules = targetRules,
                            Container = _container,
                            Context = _context,
                            MyTurn = myTurn
                        };

                        targets = await _targetingService.GetTargets(targetingRequest);
                    }

                    // step 5 get results of the activated ability
                    var activeRequest = new ActiveRequest
                    {
                        MyTurn = myTurn,
                        AbilityName = selectedAbility.Name,
                        AbilityLevel = selectedAbility.AbilityLevel,
                        Targets = targets
                    };

                    activeResults = await selectedAbility.Activation(activeRequest);
                }

                // step 10 do all damage requests
                var damageRequests = activeResults.DamageRequests;

                await HandleDamage(damageRequests);

                _messages.Append(activeResults.SB.ToString());
                _messages.Append(activeResults.StatusEffectResults.SB);

                if (!string.IsNullOrEmpty(_messages.ToString()))
                {
                    await _context.Channel.SendMessageAsync(_messages.ToString());
                    _messages.Clear();
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// method for counting down status effects
        /// </summary>
        /// <param name="myTurn">creature who is having statuses counted down</param>
        private async Task CountdownStatuses(ulong myTurn)
        {
            try
            {
                var me = _container.Creatures
                    .Where(x => x.CreatureID == myTurn)
                    .FirstOrDefault();

                me.Statuses
                    .ForEach(x => x.ReduceDuration(new ReduceDurationRequest()));
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// method for selecting abilities
        /// </summary>
        /// <param name="myTurn">creature who's turn it is</param>
        /// <returns>an ability that is available to the player</returns>
        private async Task<Ability> SelectAbility(ulong myTurn)
        {
            try
            {
                Ability selectedAbility = null;

                var me = _container.Creatures
                    .Where(x => x.CreatureID == myTurn)
                    .FirstOrDefault();

                if (me.User == 947509644706869269)
                {
                    //TODO move this to AI Service

                    selectedAbility = me.Abilities
                        .Where(x => string
                            .Equals(x.Name, "Basic Attack", StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();
                }
                else
                {
                    while (selectedAbility == null)
                    {
                        var abilityOptions = me.Abilities
                            .Where(x => x.IsActive)
                            .ToList();

                        _messages.AppendLine(
                            $"<@{me.User}> please select an ability.");

                        for (int i = 0; i < abilityOptions.Count; i++)
                        {
                            _messages.AppendLine($"{i + 1}: {abilityOptions[i].Name}");
                        }

                        var playerResponseRequest = new PlayerResponseRequest
                        {
                            User = me.User,
                            Context = _context,
                            Message = _messages.ToString(),
                        };

                        var response = await _chatService
                            .GetPlayerResponse(playerResponseRequest);

                        _messages.Clear();

                        if (response == null)
                        {
                            throw new Exception($"<@{me.User}> has left the battle");
                        }
                        else if (!int.TryParse(response, out int intResponse) ||
                                 intResponse > abilityOptions.Count() ||
                                 intResponse < 1)
                        {
                            _messages
                                .AppendLine($"<@{me.User}> please enter a valid selection");
                        }
                        else
                        {
                            selectedAbility = abilityOptions[intResponse - 1];
                        }
                    }
                }

                return selectedAbility;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// method for dealing damage to targets
        /// </summary>
        /// <param name="damageRequests">request object dictating how to deal damage to target</param>
        public async Task HandleDamage(List<DamageRequest> damageRequests)
        {
            try
            {
                // TODO get defense values

                var targetList = damageRequests
                    .Select(x => x.Target)
                    .Distinct()
                    .ToList();

                foreach (var target in targetList)
                {
                    var me = _container.Creatures
                        .Where(x => x.CreatureID == target)
                        .FirstOrDefault();

                    var requests = damageRequests
                        .Where(x => x.Target == target)
                        .ToList();
                    int totalDamage = 0;

                    var physicalReqeusts = requests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.Physical)
                        .ToList();

                    var physicalDamageRequests = physicalReqeusts
                        .Select(x => new DamageFactoryRequest()
                        {
                            Target = me,
                            AttackType = AttackTypeEnum.Physical,
                            Damage = x.Damage,
                            Penetration = x.Penetration
                        })
                        .ToList();

                    int physicalDamage = physicalDamageRequests
                        .Sum(x => _damageFactory
                            .CalculateDamage(x)
                            .GetAwaiter()
                            .GetResult());

                    var elementalRequests = requests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.Magic)
                        .ToList();

                    var elementalDamageRequests = elementalRequests
                        .Select(x => new DamageFactoryRequest()
                        {
                            Target = me,
                            AttackType = AttackTypeEnum.Magic,
                            Damage = x.Damage,
                            Penetration = x.Penetration
                        })
                        .ToList();

                    int elementalDamage = elementalDamageRequests
                        .Sum(x => _damageFactory
                            .CalculateDamage(x)
                            .GetAwaiter()
                            .GetResult());

                    var trueRequests = requests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.True)
                        .ToList();

                    var trueDamageRequests = trueRequests
                        .Select(x => new DamageFactoryRequest
                        {
                            Target = me,
                            AttackType = AttackTypeEnum.True,
                            Damage = x.Damage,
                            Penetration = x.Penetration
                        });

                    var trueDamage = trueDamageRequests
                        .Sum(x => _damageFactory
                            .CalculateDamage(x)
                            .GetAwaiter()
                            .GetResult());

                    totalDamage = physicalDamage + elementalDamage + trueDamage;

                    var elementalShields = me.Statuses
                        .Where(x =>
                            x.EffectType == EffectTypes.ElementalShield)
                        .OrderBy(x => x.Duration)
                        .ToList();

                    if (elementalShields != null && elementalShields.Count > 0)
                    {
                        var totalElementalShield = elementalShields
                            .Select(x => x.Value)
                            .Sum();

                        _messages
                            .AppendLine($"<@{me.User}>'s {me.Name} currently has {totalElementalShield} Elemental Shield");
                    }

                    var physicalShields = me.Statuses
                        .Where(x =>
                            x.EffectType == EffectTypes.PhysicalShield)
                        .OrderBy(x => x.Duration)
                        .ToList();

                    if (physicalShields != null && physicalShields.Count > 0)
                    {
                        var totalPhysicalShield = physicalShields
                            .Select(x => x.Value)
                            .Sum();

                        _messages
                            .AppendLine($"<@{me.User}>'s {me.Name} currently has {totalPhysicalShield} Physical Shield");
                    }

                    var generalShields = me.Statuses
                        .Where(x =>
                            x.EffectType == EffectTypes.GeneralShield)
                        .OrderBy(x => x.Duration)
                        .ToList();

                    if (generalShields != null && generalShields.Count > 0)
                    {
                        var totalGeneralShield = generalShields
                            .Select(x => x.Value)
                            .Sum();

                        _messages
                            .AppendLine($"<@{me.User}>'s {me.Name} currently has {totalGeneralShield} General Shield");
                    }

                    int remainingDamage = 0;

                    int remainingElementalDamage = elementalDamage;
                    int remainingPhysicalDamage = physicalDamage;

                    while (elementalShields != null &&
                           elementalShields.Count > 0 &&
                           remainingElementalDamage > 0)
                    {
                        var currentShield = elementalShields.First();

                        if (currentShield.Value > remainingElementalDamage)
                        {
                            currentShield.Value -= remainingElementalDamage;
                            remainingElementalDamage = 0;
                        }
                        else
                        {
                            remainingElementalDamage -= currentShield.Value;
                            elementalShields.Remove(currentShield);
                        }
                    }

                    remainingDamage = remainingElementalDamage;

                    while (physicalShields != null &&
                           physicalShields.Count > 0 &&
                           remainingPhysicalDamage > 0)
                    {
                        var currentShield = physicalShields.First();

                        if (currentShield.Value > remainingPhysicalDamage)
                        {
                            currentShield.Value -= remainingPhysicalDamage;
                            remainingPhysicalDamage = 0;
                        }
                        else
                        {
                            remainingPhysicalDamage -= currentShield.Value;
                            physicalShields.Remove(currentShield);
                        }
                    }

                    remainingDamage += remainingPhysicalDamage + trueDamage;

                    while (generalShields != null &&
                           generalShields.Count > 0 &&
                           remainingDamage > 0)
                    {
                        var currentShield = generalShields.First();

                        if (currentShield.Value > remainingDamage)
                        {
                            currentShield.Value -= remainingDamage;
                            remainingDamage = 0;
                        }
                        else
                        {
                            remainingDamage -= currentShield.Value;
                            generalShields.Remove(currentShield);
                        }
                    }

                    if (remainingDamage > 0)
                    {
                        me.Stats.Health -= remainingDamage;
                    }

                    if (me.Stats.Health < 0)
                    {
                        me.Stats.Health = 0;
                    }

                    if (elementalDamage != remainingElementalDamage)
                    {
                        _messages.AppendLine(
                            $"<@{me.User}>'s {me.Name}'s Elemental Shield has blocked {elementalDamage - remainingElementalDamage} damage");
                    }

                    if (physicalDamage != remainingPhysicalDamage)
                    {
                        _messages.AppendLine(
                            $"<@{me.User}>'s {me.Name}'s Physical Shield has blocked {physicalDamage - remainingPhysicalDamage} damage");
                    }

                    if (totalDamage != remainingDamage)
                    {
                        _messages.AppendLine(
                            $"<@{me.User}>'s {me.Name}'s Elemental Shield has blocked {totalDamage - remainingDamage} damage");
                    }

                    _messages.
                        AppendLine($"<@{me.User}>'s {me.Name} took {totalDamage} total damage, {totalDamage - remainingDamage} was blocked by shields.  " +
                                   $"{remainingDamage} was dealt to their health");

                    _messages
                        .AppendLine($"{remainingElementalDamage} was Elemental Damage, {remainingPhysicalDamage} was Physical Damage, {trueDamage} was True Damage");

                    if (me.Stats.Health == 0)
                    {
                        _messages.AppendLine($"<@{me.User}>'s {me.Name} has died");
                    }

                    _messages.AppendLine(
                        $"<@{me.User}>'s {me.Name} has {me.Stats.Health} remaining health");

                    me.Statuses.RemoveAll(x => (x.EffectType == EffectTypes.ElementalShield ||
                                                x.EffectType == EffectTypes.PhysicalShield ||
                                                x.EffectType == EffectTypes.GeneralShield) && x.Value <= 0);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
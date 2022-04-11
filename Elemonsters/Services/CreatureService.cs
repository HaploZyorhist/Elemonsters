using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.StatusEffects.Requests;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for handling creatures
    /// </summary>
    public class CreatureService : ICreatureService
    {
        public IDBService _dbService;
        public StatFactory _statFactory;
        public readonly DamageFactory _damageFactory;
        private readonly ITargetingService _targetingService;

        /// <summary>
        /// ctor for adding creatures to the list of available creatures
        /// </summary>
        public CreatureService(IDBService dbService,
                               StatFactory statFactory,
                               DamageFactory damageFactory,
                               ITargetingService targetingService)
        {
            _dbService = dbService;
            _statFactory = statFactory;
            _damageFactory = damageFactory;
            _targetingService = targetingService;
        }

        /// <inheritdoc />
        public async Task<CreatureBase> GetCreatureStats(StatsRequest creatureRequest)
        {
            try
            {
                var creature = await _dbService.GetCreatureBase("TestyBoi");

                var creatureStats = await _dbService.GetCreatureStats(creatureRequest.CreatureID);

                var updatedStats = await _statFactory.GenerateStats(creatureStats);

                var creatureElements = await _dbService.GetCreatureElements(creatureRequest.CreatureID);

                creature.Stats = updatedStats;
                creature.Elements = creatureElements;
                creature.CreatureID = creatureRequest.CreatureID;

                return creature;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<AssignPassiveResult> AssignPassives(AssignPassivesRequest request)
        {
            try
            {
                var result = new AssignPassiveResult();

                //TODO continue to refactor and clean this up
                foreach (var creature in request.Creatures)
                {
                    var passives = creature.Abilities
                        .Where(x => x.PassiveAbilities.Count > 0)
                        .ToList();

                    foreach (var passive in passives)
                    {
                        var allPassives = passive.PassiveAbilities.ToList();

                        var targetRules = await passive.GetTargetOptions();

                        var targetingRequest = new GetTargetsRequest
                        {
                            Rules = targetRules,
                            Creatures = request.Creatures,
                            Context = request.Context,
                            MyTurn = creature.CreatureID
                        };

                        var targets = await _targetingService.GetTargets(targetingRequest);

                        foreach (var p in allPassives)
                        {
                            var seRequest = new AddStatusEffectRequest
                            {
                                Ability = passive,
                                Creatures = request.Creatures,
                                Targets = targets,
                                Level =  (int)creature.Rank
                            };

                            var activations = await p.AddStatusEffect(seRequest);

                            result.Messages.Append(activations.SB.ToString());
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

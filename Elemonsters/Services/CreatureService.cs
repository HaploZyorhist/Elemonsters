using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for handling creatures
    /// </summary>
    public class CreatureService : ICreatureService
    {
        public IDBService _dbService;

        /// <summary>
        /// ctor for adding creatures to the list of available creatures
        /// </summary>
        public CreatureService(IDBService dbService)
        {
            _dbService = dbService;
        }

        /// <inheritdoc />
        public async Task<CreatureBase> GetCreatureStats(StatsRequest creatureRequest)
        {
            StatFactory _statFactory = new StatFactory();

            try
            {
                var creature = await _dbService.GetCreatureBase(creatureRequest.CreatureName);

                var creatureStats = await _dbService.GetCreatureStats(creatureRequest.CreatureID);

                var updatedStats = await _statFactory.GenerateStats(creatureStats);

                var creatureElements = await _dbService.GetCreatureElements(creatureRequest.CreatureID);

                creature.Stats = updatedStats;
                creature.Elements = creatureElements;

                if (creatureRequest.CreatureID != 0)
                {
                    creature.User = 161700527531491328;
                }

                return creature;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

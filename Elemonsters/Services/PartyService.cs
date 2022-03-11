using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// class for handling party related services
    /// </summary>
    public class PartyService : IPartyService
    {
        public ICreatureService _creatureService;
        public IDBService _dbService;

        public PartyService(ICreatureService creatureService,
                            IDBService dBService)
        {
            _creatureService = creatureService;
            _dbService = dBService;
        }

        /// <inheritdoc />
        public async Task<List<CreatureBase>> GetParty(ulong playerID)
        {
            try
            {
                var party = new List<CreatureBase>();

                var partyRequests = await _dbService.GetPlayerParty(playerID);

                foreach (var creatureRequest in partyRequests)
                {
                    var creature = await _creatureService.GetCreatureStats(creatureRequest);

                    creature.User = playerID;

                    party.Add(creature);
                }

                return party;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

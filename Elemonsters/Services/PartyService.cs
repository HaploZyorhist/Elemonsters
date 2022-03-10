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
                //TODO get party from dbservice
                List<ulong> partyIDs = new List<ulong>();

                var party = new List<CreatureBase>();

                foreach (var creatureID in partyIDs)
                {
                    var creature = await _creatureService.GetCreatureStats(creatureID);

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

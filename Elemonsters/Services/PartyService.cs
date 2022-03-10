using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services
{
    /// <summary>
    /// class for handling party related services
    /// </summary>
    public class PartyService : IPartyService
    {
        public StatFactory _statFactory = new StatFactory();
        public ICreatureService _creatureService;

        public PartyService(ICreatureService creatureService)
        {
            _creatureService = creatureService;
        }

        /// <inheritdoc />
        public async Task<List<CreatureBase>> GetParty(ulong playerID)
        {
            try
            {
                List<CreatureBase> party = new List<CreatureBase>();

                Dictionary<ulong, string> partyMembers = new Dictionary<ulong, string>();
                partyMembers.Add(0, "Testy");

                foreach (var pM in partyMembers)
                {
                    var cList = await _creatureService.GetCreatureList();
                    var c = cList.Where(x => x.Key == pM.Value).FirstOrDefault().Value;

                    var cType = c.GetType();
                    var nc = (CreatureBase)Activator.CreateInstance(cType);

                    var newCreature = await _creatureService.GetCreatureStats(playerID, nc);

                    party.Add(newCreature);
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

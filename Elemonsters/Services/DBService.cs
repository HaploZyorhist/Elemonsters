using Elemonsters.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for interacting with the database
    /// </summary>
    public class DBService : IDBService
    {
        /// <inheritdoc />
        public async Task<List<ulong>> GetPlayerParty(ulong playerId)
        {
            try
            {
                var creatureIDs = new List<ulong>();

                creatureIDs.Add(1);

                return creatureIDs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

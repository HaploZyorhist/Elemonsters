using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services.Interfaces
{
    /// <summary>
    /// interface for interacting with the database to get information
    /// </summary>
    public interface IDBService
    {
        /// <summary>
        /// method for getting the player's party from the database
        /// </summary>
        /// <param name="playerID">the id of the pleyer being queried</param>
        /// <returns>list of creature id's in the party</returns>
        Task<List<ulong>> GetPlayerParty(ulong playerID);
    }
}

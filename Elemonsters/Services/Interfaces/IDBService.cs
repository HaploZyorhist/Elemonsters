using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Requests;

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
        /// <param name="playerID">the id of the player being queried</param>
        /// <returns>list of creature id's in the party</returns>
        Task<List<StatsRequest>> GetPlayerParty(ulong playerID);

        /// <summary>
        /// method for getting stats for a creature from database
        /// </summary>
        /// <param name="creatureID">id of the creature being queried</param>
        /// <returns>stats object for a creature</returns>
        Task<CreatureStats> GetCreatureStats(ulong creatureID);

        /// <summary>
        /// gets the base creature from the database
        /// </summary>
        /// <param name="creature">the base for a new creature</param>
        /// <param name="creatureRequest">the request for the creature to create</param>
        /// <returns>creature base object containing base setup for a creature</returns>
        Task<CreatureBase> GetCreatureBase(CreatureBase creature, StatsRequest creatureRequest);

        /// <summary>
        /// gets the elements assigned to a creature from the database
        /// </summary>
        /// <param name="creatureID">the id of the creature being queried</param>
        /// <returns>creature elements object containing their element setup</returns>
        Task<CreatureElements> GetCreatureElements (ulong creatureID);
    }
}

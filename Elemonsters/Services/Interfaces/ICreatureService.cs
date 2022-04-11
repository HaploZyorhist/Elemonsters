using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;

namespace Elemonsters.Services.Interfaces
{
    /// <summary>
    /// interface for handling the creature service
    /// </summary>
    public interface ICreatureService
    {
        /// <summary>
        /// method for getting stats for an individual creature from the database
        /// </summary>
        /// <param name="creatureRequest">object containing the name and the id of the creature instance being queried</param>
        /// <returns>creature with correct stats</returns>
        Task<CreatureBase> GetCreatureStats(StatsRequest creatureRequest);

        /// <summary>
        /// method for assigning creature passives to the creatures
        /// </summary>
        /// <param name="request">object containing request information for assigning passives</param>
        /// <returns>object with messages on what happened in the method</returns>
        Task<AssignPassiveResult> AssignPassives(AssignPassivesRequest request);
    }
}

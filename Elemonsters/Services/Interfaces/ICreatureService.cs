using Elemonsters.Assets.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;

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
    }
}

using Elemonsters.Assets.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Factories
{
    /// <summary>
    /// factory for generating stats for a creature
    /// </summary>
    public class StatFactory
    {
        /// <summary>
        /// method for generating stats for an instance from the base
        /// </summary>
        public async Task<CreatureStats> GenerateStats (CreatureStats stats)
        {
            try
            {
                var newStats = new CreatureStats ();

                newStats = stats;

                return newStats;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

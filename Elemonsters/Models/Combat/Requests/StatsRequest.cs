using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object for applying a status effect to a creature
    /// </summary>
    public class StatsRequest
    {
        /// <summary>
        /// name of creature who stats are being gotten for
        /// </summary>
        public string CreatureName { get; set; }

        /// <summary>
        /// id of creature who the stats are being gotten for
        /// </summary>
        public ulong CreatureID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Results;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing data for requesting an ability activation
    /// </summary>
    public class ActiveRequest
    {
        /// <summary>
        /// list of targets selected by the player
        /// </summary>
        public List<CreatureBase> Targets { get; set; } = new List<CreatureBase>();

        /// <summary>
        /// id of creature who is actively taking turn
        /// </summary>
        public ulong MyTurn { get; set; }

        /// <summary>
        /// name of the ability being activated
        /// </summary>
        public string AbilityName { get; set; } = "";

        /// <summary>
        /// level of the ability being activated
        /// </summary>
        public int AbilityLevel { get; set; } = 0;
    }
}

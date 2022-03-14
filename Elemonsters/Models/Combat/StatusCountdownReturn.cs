using Elemonsters.Assets.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat
{
    /// <summary>
    /// object containing results of status effect countdown
    /// </summary>
    public class StatusCountdownReturn
    {
        /// <summary>
        /// creature who's status effects are counting down
        /// </summary>
        public CreatureBase MyTurn { get; set; }

        /// <summary>
        /// message from statuses that are being counted down
        /// </summary>
        public StringBuilder SB { get; set; } = new StringBuilder();
    }
}

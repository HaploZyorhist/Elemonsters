using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing details on activating a status effect
    /// </summary>
    public class ActivateStatusEffectRequest
    {
        /// <summary>
        /// object containing all of the details of the battle
        /// </summary>
        public BattleContainer Container { get; set; } = new BattleContainer();

        /// <summary>
        /// creature id of the user
        /// </summary>
        public ulong MyTurn { get; set; }

        /// <summary>
        /// list of creature ids of the targets to be hit
        /// </summary>
        public List<ulong> Targets { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing details on how to activate a passive effect
    /// </summary>
    public class PassiveRequest
    {
        /// <summary>
        /// id of creature who is actively taking turn
        /// </summary>
        public ulong MyTurn { get; set; }

        /// <summary>
        /// object containing all of the details of the battle
        /// </summary>
        public BattleContainer Container { get; set; } = new BattleContainer();

        /// <summary>
        /// targets being affected by the activation of the passive
        /// </summary>
        public CreatureBase Target { get; set; }

        /// <summary>
        /// name of ability being activated
        /// </summary>
        public string AbilityName { get; set; } = "";

        /// <summary>
        /// level of ability being activated
        /// </summary>
        public int AbilityLevel { get; set; } = 0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// request object for getting targets for an ability
    /// </summary>
    public class GetTargetsRequest
    {
        /// <summary>
        /// object containing all of the details of the battle
        /// </summary>
        public BattleContainer Container { get; set; } = new BattleContainer();
        
        /// <summary>
        /// creature who is currently activating the ability
        /// </summary>
        public ulong MyTurn { get; set; }
    }
}

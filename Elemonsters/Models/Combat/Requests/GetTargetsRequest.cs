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
        /// list of available targets
        /// </summary>
        public List<CreatureBase> Targets { get; set; } = new List<CreatureBase>();
        
        /// <summary>
        /// creature who is currently activating the ability
        /// </summary>
        public CreatureBase MyTurn { get; set; }
    }
}

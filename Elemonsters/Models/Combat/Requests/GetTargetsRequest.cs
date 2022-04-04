using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Results;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// request object for getting targets for an ability
    /// </summary>
    public class GetTargetsRequest
    {
        /// <summary>
        /// result from the abilities targeting method
        /// </summary>
        public TargetRulesResult Rules { get; set; } = new TargetRulesResult();

        /// <summary>
        /// list of creatures involved in the battle
        /// </summary>
        public List<CreatureBase> Creatures { get; set; } = new List<CreatureBase>();

        /// <summary>
        /// command context for passing to chat service
        /// </summary>
        public ICommandContext Context { get; set; }

        /// <summary>
        /// creature who is currently activating the ability
        /// </summary>
        public ulong MyTurn { get; set; }
    }
}

using Discord.Commands;
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
        /// object containing all of the details of the battle
        /// </summary>
        public BattleContainer Container { get; set; } = new BattleContainer();

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

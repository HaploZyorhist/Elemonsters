using Discord.Commands;
using Elemonsters.Models.Combat;

namespace Elemonsters.Services.Interfaces
{
    /// <summary>
    /// interface for handling battle service requests
    /// </summary>
    public interface IBattleService
    {
        /// <summary>
        /// method for handling battles
        /// </summary>
        /// <param name="context">the discord context that contains data on the initiating request</param>
        /// <param name="battleContainer">object containing the details of the battle to be performed</param>
        Task BeginBattle(ICommandContext context, BattleContainer battleContainer);
    }
}

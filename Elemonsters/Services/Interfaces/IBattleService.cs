using Discord.Commands;
using Elemonsters.Assets.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="context">object containing the details of the initiation of the command</param>
        /// <param name="player1Party">the party that the first player is using</param>
        /// <param name="player2Party">the party that the second player is using</param>
        /// <returns></returns>
        Task BeginBattle(ICommandContext context, List<CreatureBase> player1Party, List<CreatureBase> player2Party);
    }
}

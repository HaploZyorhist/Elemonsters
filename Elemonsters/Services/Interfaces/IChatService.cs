using Elemonsters.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services.Interfaces
{
    /// <summary>
    /// interface for chat service
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// method for getting player response or timeout
        /// </summary>
        /// <param name="request">request object containing details for the player</param>
        /// <returns>the player's typed response</returns>
        Task<string> GetPlayerResponse(PlayerResponseRequest request);
    }
}

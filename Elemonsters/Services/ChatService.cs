using Discord;
using Discord.Commands;
using Elemonsters.Models.Chat;
using Elemonsters.Services.Interfaces;
using Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for handling chat interactions with users
    /// </summary>
    public class ChatService : IChatService
    {
        public InteractivityService _interact;

        /// <summary>
        /// constructor for chat service
        /// </summary>
        public ChatService(InteractivityService interact)
        {
            _interact = interact;
        }

        /// <inheritdoc />
        public async Task<string> GetPlayerResponse(PlayerResponseRequest request)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(request.Message);

                await request.Context.Channel.SendMessageAsync(sb.ToString());

                var responseRaw = await _interact.NextMessageAsync(x => x.Author.Id == request.User &&
                                                                  x.Channel == request.Context.Channel);

                if (responseRaw.IsTimeouted == true)
                {
                    throw new Exception("response timed out");
                }

                var playerResponse = responseRaw.Value.ToString().ToLower();

                return playerResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

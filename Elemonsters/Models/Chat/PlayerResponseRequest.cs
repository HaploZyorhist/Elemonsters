using Discord.Commands;

namespace Elemonsters.Models.Chat
{
    /// <summary>
    /// object containing request contents for getting player response
    /// </summary>
    public class PlayerResponseRequest
    {
        /// <summary>
        /// context of the chat
        /// </summary>
        public ICommandContext Context { get; set; }

        /// <summary>
        /// user who the response is for
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// message given to the player
        /// </summary>
        public string Message { get; set; }
    }
}

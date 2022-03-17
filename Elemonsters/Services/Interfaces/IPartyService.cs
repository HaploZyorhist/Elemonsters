using Elemonsters.Assets.Creatures;

namespace Elemonsters.Services.Interfaces
{
    /// <summary>
    /// interface for performing party related functions
    /// </summary>
    public interface IPartyService
    {
        /// <summary>
        /// method for getting the player's party
        /// </summary>
        /// <param name="playerID">the id of the player for the party</param>
        /// <returns>a list of creatures that the player has in their party</returns>
        Task<List<CreatureBase>> GetParty(ulong playerID);
    }
}

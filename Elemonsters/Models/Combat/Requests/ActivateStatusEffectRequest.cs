using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing details on activating a status effect
    /// </summary>
    public class ActivateStatusEffectRequest
    {
        /// <summary>
        /// list of creatures involved in the battle
        /// </summary>
        public List<CreatureBase> Creatures { get; set; } = new List<CreatureBase>();

        /// <summary>
        /// creature id of the user
        /// </summary>
        public ulong MyTurn { get; set; }

        /// <summary>
        /// list of creature ids of the targets to be hit
        /// </summary>
        public List<ulong> Targets { get; set; } = new List<ulong>();

        /// <summary>
        /// the value of the request
        /// </summary>
        public int Value { get; set; }
    }
}

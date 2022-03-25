using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing data for requesting an ability activation
    /// </summary>
    public class ActiveRequest
    {
        public BattleContainer Container { get; set; } = new BattleContainer();
        /// <summary>
        /// list of targets selected by the player
        /// </summary>
        public List<ulong> Targets { get; set; }

        /// <summary>
        /// id of creature who is actively taking turn
        /// </summary>
        public ulong MyTurn { get; set; }

        /// <summary>
        /// name of the ability being activated
        /// </summary>
        public string AbilityName { get; set; } = "";

        /// <summary>
        /// level of the ability being activated
        /// </summary>
        public int AbilityLevel { get; set; } = 0;
    }
}

using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat.Results
{
    /// <summary>
    /// result object containing the options for targeting
    /// </summary>
    public class GetTargetsResult
    {
        /// <summary>
        /// how many targets can be selected
        /// </summary>
        public int TotalTargets { get; set; }

        /// <summary>
        /// how many targets can be selected from the first option list
        /// </summary>
        public int FirstOptionTargets { get; set; }

        /// <summary>
        /// how many targets can be selected from the second option list
        /// </summary>
        public int SecondOptionTargets { get; set; }

        /// <summary>
        /// how many targets can be selected from the third option list
        /// </summary>
        public int ThirdOptionTargets { get; set; }

        /// <summary>
        /// list of targets for first option
        /// </summary>
        public List<CreatureBase> FirstOption { get; set; } = new List<CreatureBase>();

        /// <summary>
        /// list of targets for second option
        /// </summary>
        public List<CreatureBase> SecondOption { get; set; } = new List<CreatureBase>();

        /// <summary>
        /// list of options for third option
        /// </summary>
        public List<CreatureBase> ThirdOption { get; set; } = new List<CreatureBase>();
    }
}

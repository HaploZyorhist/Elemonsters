using Elemonsters.Models.Enums;

namespace Elemonsters.Models
{
    /// <summary>
    /// class for containing data on what the user is doing
    /// </summary>
    public class UserLockData
    {
        /// <summary>
        /// bool containing whether the user is locked in an action
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// guid for tracking what instance of an activity the user is doing
        /// </summary>
        public Guid Instance { get; set; }

        /// <summary>
        /// action the user is locked doing
        /// </summary>
        public ActivityEnum Activity { get; set; }
    }
}

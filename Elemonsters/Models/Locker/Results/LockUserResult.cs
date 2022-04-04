using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Locker.Results
{
    /// <summary>
    /// result object containing data on the users lock status
    /// </summary>
    public class LockUserResult
    {
        /// <summary>
        /// discord user id
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// activity the user is performing
        /// </summary>
        public ActivityEnum Activity { get; set; }

        /// <summary>
        /// indicator if user is currently locked
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// instance the user is locked to
        /// </summary>
        public Guid Instance { get; set; }
    }
}

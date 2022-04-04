using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Locker.Results
{
    /// <summary>
    /// result object containing data on the user's lock
    /// </summary>
    public class CheckLockResult
    {
        /// <summary>
        /// discord user id
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// activity the user is doing
        /// </summary>
        public ActivityEnum Activity { get; set; }

        /// <summary>
        /// instance the user is locked to
        /// </summary>
        public Guid Instance { get; set; }

        /// <summary>
        /// indicator if the user is locked
        /// </summary>
        public bool IsLocked { get; set; }
    }
}

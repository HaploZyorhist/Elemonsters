using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Locker.Results
{
    /// <summary>
    /// result object indicating user lock status
    /// </summary>
    public class UnlockUserResult
    {
        /// <summary>
        /// discord user id
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// activity the user is currently performing
        /// </summary>
        public ActivityEnum Activity { get; set; }

        /// <summary>
        /// instance the user is locked to
        /// </summary>
        public Guid Instance { get; set; }

        /// <summary>
        /// indicator if user is locked
        /// </summary>
        public bool IsLocked { get; set; }
    }
}

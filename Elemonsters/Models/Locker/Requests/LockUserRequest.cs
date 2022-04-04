using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Locker.Requests
{
    /// <summary>
    /// object containing data on locking the user
    /// </summary>
    public class LockUserRequest
    {
        /// <summary>
        /// discord user id
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// activity to lock the user doing
        /// </summary>
        public ActivityEnum Activity { get; set; }

        /// <summary>
        /// instance the user is being locked to
        /// </summary>
        public Guid Instance { get; set; }
    }
}

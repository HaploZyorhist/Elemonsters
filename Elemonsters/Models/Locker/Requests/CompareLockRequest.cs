using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Locker.Requests
{
    /// <summary>
    /// object containing data on comparing user lock
    /// </summary>
    public class CompareLockRequest
    {
        /// <summary>
        /// discord user id
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// activity the user is supposed to be doing
        /// </summary>
        public ActivityEnum Activity { get; set; }

        /// <summary>
        /// instance the user is supposed to be in
        /// </summary>
        public Guid Instance { get; set; }
    }
}

namespace Elemonsters.Models.Locker.Requests
{
    /// <summary>
    /// object containing data on how to check user lock
    /// </summary>
    public class CheckLockRequest
    {
        /// <summary>
        /// discord user id
        /// </summary>
        public ulong User { get; set; }
    }
}

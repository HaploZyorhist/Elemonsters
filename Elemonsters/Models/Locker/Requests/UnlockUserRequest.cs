namespace Elemonsters.Models.Locker.Requests
{
    /// <summary>
    /// object containing data on unlocking the user
    /// </summary>
    public class UnlockUserRequest
    {
        /// <summary>
        /// discord user id
        /// </summary>
        public ulong User { get; set; }
    }
}

namespace Elemonsters.Models.Locker.Results
{
    /// <summary>
    /// result object containing data on the comparison of user lock
    /// </summary>
    public class CompareLockResult
    {
        /// <summary>
        /// indicator of if locks match
        /// </summary>
        public bool LockMatch { get; set; } = false;
    }
}

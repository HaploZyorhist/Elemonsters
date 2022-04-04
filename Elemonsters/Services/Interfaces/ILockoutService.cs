using Discord;
using Elemonsters.Models.Enums;
using Elemonsters.Models.Locker.Requests;
using Elemonsters.Models.Locker.Results;

namespace Elemonsters.Services.Interfaces
{
    public interface ILockoutService
    {
        /// <summary>
        /// checks if the user is currently performing an activity
        /// </summary>
        /// <param name="request">object containing details on user being checked</param>
        Task<CheckLockResult> CheckLock(CheckLockRequest request);

        /// <summary>
        /// checks if the user is currently performing an activity
        /// </summary>
        /// <param name="request">object containing details on the lock to be checked</param>
        Task<CompareLockResult> CompareLock(CompareLockRequest request);

        /// <summary>
        /// task for locking a user
        /// </summary>
        /// <param name="request">object containing details on how to lock the user</param>
        Task<LockUserResult> LockUser(LockUserRequest request);

        /// <summary>
        /// task for unlocking a user
        /// </summary>
        /// <param name="request">object containing details on unlocking the user</param>
        Task<UnlockUserResult> UnlockUser(UnlockUserRequest request);
    }
}

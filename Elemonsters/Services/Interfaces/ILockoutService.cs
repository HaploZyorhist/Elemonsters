using Discord;
using Elemonsters.Models.Enums;

namespace Elemonsters.Services.Interfaces
{
    public interface ILockoutService
    {
        /// <summary>
        /// checks if the user is currently performing an activity
        /// </summary>
        /// <param name="user">discord user information</param>
        Task<bool> CheckGeneralLock(IUser user);

        /// <summary>
        /// checks if the user is currently performing the activity given with the instance given
        /// </summary>
        /// <param name="user">discord user information</param>
        /// <param name="activity">activity from enum</param>
        /// <param name="instance">instance of the activity being performed</param>
        Task<bool> CheckActivityLock(IUser user, ActivityEnum activity, int instance);

        /// <summary>
        /// task for locking a user
        /// </summary>
        /// <param name="user">discord user information</param>
        /// <param name="activity">activity from enum</param>
        /// <param name="instance">instance of the activity being performed</param>
        Task<bool> LockUser(IUser user, ActivityEnum activity, int instance);

        /// <summary>
        /// task for unlocking a user
        /// </summary>
        /// <param name="user">discord user information</param>
        Task<bool> UnlockUser(IUser user);
    }
}

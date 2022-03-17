using System.Collections.Concurrent;
using Discord;
using Elemonsters.Models;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// Service for locking a player out
    /// </summary>
    public class LockoutService : ILockoutService
    {
        /// <summary>
        /// thread safe list of users and their status
        /// </summary>
        public ConcurrentDictionary<ulong, UserLockData> lockout = new ();

        #region Locking/Unlocking

        /// <inheritdoc />
        public async Task<bool> CheckGeneralLock(IUser user)
        {
            try
            {
                // try to add user to the list
                var ins = lockout.TryAdd(user.Id, new UserLockData
                {
                    IsLocked = false,
                    Activity = ActivityEnum.None,
                    Instance = 0
                });

                if (ins == true)
                {
                    return false;
                }

                // if user was already in list get user
                var userDetails = lockout[user.Id];

                // check if user is locked
                if (!userDetails.IsLocked)
                {
                    return false;
                }

                // user was locked
                return true;
            }
            catch (Exception ex)
            {
                // something bad happened, assume they are not locked
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<bool> CheckActivityLock(IUser user, ActivityEnum activity, int instance)
        {
            try
            {
                // try to add user to the list
                var ins = lockout.TryAdd(user.Id, new UserLockData
                {
                    IsLocked = false,
                    Activity = ActivityEnum.None,
                    Instance = 0
                });

                if (ins == true)
                {
                    return false;
                }

                // if user was already in list get user
                var userDetails = lockout[user.Id];

                // check if user is doing what they are supposed to be doing
                if (!userDetails.IsLocked ||
                    userDetails.Instance != instance ||
                    userDetails.Activity != activity)
                {
                    return false;
                }

                // user was locked and doing what they are supposed to be doing
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<bool> LockUser(IUser user, ActivityEnum activity, int instance)
        {
            try
            {
                // todo: add ban checker to ensure the user isn't banned from playing

                // try to add user to the list
                var ins = lockout.TryAdd(user.Id, new UserLockData
                {
                    IsLocked = true,
                    Activity = activity,
                    Instance = instance
                });

                // user successfully added and locked
                if (ins == true)
                {
                    return true;
                }

                // if user was already in list get user
                var userDetails = lockout[user.Id];

                // user already locked doing something else
                if (userDetails.IsLocked)
                {
                    return false;
                }

                // otherwise lock user
                lockout[user.Id].Instance = instance;
                lockout[user.Id].Activity = activity;
                lockout[user.Id].IsLocked = true;

                // user was successfully locked
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<bool> UnlockUser(IUser user)
        {
            try
            {
                // try to add user to the list
                var ins = lockout.TryAdd(user.Id, new UserLockData
                {
                    IsLocked = false,
                    Activity = ActivityEnum.None,
                    Instance = 0
                });

                // user was not in the list
                if (ins == true)
                {
                    return false;
                }

                // if user was already in list get user
                var userDetails = lockout[user.Id];

                // user was not locked
                if (!userDetails.IsLocked)
                {
                    return false;
                }

                lockout[user.Id].Instance = 0;
                lockout[user.Id].Activity = ActivityEnum.None;
                lockout[user.Id].IsLocked = false;

                // user was locked
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}
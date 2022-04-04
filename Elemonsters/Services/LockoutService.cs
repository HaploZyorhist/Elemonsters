using System.Collections.Concurrent;
using Elemonsters.Models;
using Elemonsters.Models.Enums;
using Elemonsters.Models.Locker.Requests;
using Elemonsters.Models.Locker.Results;
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
        public async Task<CheckLockResult> CheckLock(CheckLockRequest request)
        {
            try
            {
                // try to add user to the list
                lockout.TryAdd(request.User, new UserLockData
                {
                    IsLocked = false,
                    Activity = ActivityEnum.None,
                    Instance = new Guid()
                });

                // if user was already in list get user
                var userDetails = lockout[request.User];

                var result = new CheckLockResult
                {
                    User = request.User,
                    IsLocked = userDetails.IsLocked,
                    Activity = userDetails.Activity,
                    Instance = userDetails.Instance
                };

                // user was locked
                return result;
            }
            catch (Exception ex)
            {
                // something bad happened, assume they are not locked
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<CompareLockResult> CompareLock(CompareLockRequest request)
        {
            try
            {
                // try to add user to the list
                lockout.TryAdd(request.User, new UserLockData
                {
                    IsLocked = false,
                    Activity = ActivityEnum.None,
                    Instance = new Guid()
                });

                // if user was already in list get user
                var userDetails = lockout[request.User];

                var result = new CompareLockResult();

                if (request.Activity == userDetails.Activity &&
                    request.Instance == userDetails.Instance &&
                    userDetails.IsLocked)
                {
                    result.LockMatch = true;
                }

                // user was locked
                return result;
            }
            catch (Exception ex)
            {
                // something bad happened, assume they are not locked
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<LockUserResult> LockUser(LockUserRequest request)
        {
            try
            {
                // todo: add ban checker to ensure the user isn't banned from playing

                // try to add user to the list
                lockout.TryAdd(request.User, new UserLockData
                {
                    IsLocked = true,
                    Activity = request.Activity,
                    Instance = request.Instance
                });

                // if user was already in list get user
                var userDetails = lockout[request.User];

                // if user is not currently locked, update lock
                if (!userDetails.IsLocked)
                {
                    lockout[request.User].IsLocked = true;
                    lockout[request.User].Activity = request.Activity;
                    lockout[request.User].Instance = request.Instance;
                }

                var result = new LockUserResult
                {
                    User = request.User,
                    Activity = lockout[request.User].Activity,
                    IsLocked = lockout[request.User].IsLocked,
                    Instance = lockout[request.User].Instance
                };

                // user was successfully locked
                return result;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<UnlockUserResult> UnlockUser(UnlockUserRequest request)
        {
            try
            {
                // try to add user to the list
                lockout.TryAdd(request.User, new UserLockData
                {
                    IsLocked = false,
                    Activity = ActivityEnum.None,
                    Instance = new Guid()
                });

                // update user lock data
                lockout[request.User].Activity = ActivityEnum.None;
                lockout[request.User].IsLocked = false;
                lockout[request.User].Instance = new Guid();

                var userData = lockout[request.User];

                var result = new UnlockUserResult
                {
                    User = request.User,
                    Activity = userData.Activity,
                    IsLocked = userData.IsLocked,
                    Instance = userData.Instance
                };

                // user was locked
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
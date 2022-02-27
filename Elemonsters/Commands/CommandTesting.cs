using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Elemonsters.Models;
using Elemonsters.Services;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Commands
{
    /// <summary>
    /// Group of basic commands
    /// </summary>
    [Group("Test")]
    [Summary("Group of basic commands")]
    public class CommandTesting : ModuleBase
    {

        #region Fields

        public ActivityEnum _activity = ActivityEnum.Testing;
        public ILockoutService _locker;
        private InstanceTrackerService _instance;

        #endregion

        #region CTOR

        public CommandTesting(ILockoutService locker,
                              InstanceTrackerService instance)
        {
            _locker = locker;
            _instance = instance;
        }

        #endregion

        #region Commands

        [Command("Cancel")]
        [Summary("This command is used for cancelling user lockout")]
        public async Task CancelCommand()
        {
            var context = Context;
            try
            {
                var success = await _locker.UnlockUser(context.User);

                if (!success)
                {
                    throw new Exception();
                }

                await ReplyAsync($"{context.User.Mention} was unlocked");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"{nameof(CancelCommand)} has failed");
            }
        }

        [Command("Lock")]
        [Summary("This command is used for lock testing and instance checking")]
        public async Task LockingCommand()
        {
            var sb = new StringBuilder();
            var context = Context;
            try
            {
                var userCheck = await _locker.CheckGeneralLock(context.User);

                if (userCheck == false)
                {
                    sb.AppendLine($"Test 1: {context.User.Mention} was not locked");
                }
                else
                {
                    sb.AppendLine($"Test 1: {context.User.Mention} was locked");
                }

                var userInstanceCheck = await _locker.CheckActivityLock(context.User, _activity, _instance.Instance);

                if (userInstanceCheck == false)
                {
                    sb.AppendLine($"Test 2: {context.User.Mention} was not performing {_activity} for the instance {_instance.Instance}");
                }
                else
                {
                    sb.AppendLine($"Test 2: {context.User.Mention} was performing {_activity} specified for instance {_instance.Instance}");
                }

                var userLockCheck = await _locker.LockUser(context.User, _activity, _instance.Instance);

                if (userLockCheck == false)
                {
                    sb.AppendLine($"Test 3: Unable to lock {context.User.Mention} with the instance {_instance.Instance} and activity {_activity}");
                }
                else
                {
                    sb.AppendLine($"Test 3: {context.User.Mention} was locked with the instance {_instance.Instance} and activity {_activity}");
                }

                await ReplyAsync(sb.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync($"{nameof(LockingCommand)} has failed");
            }
        }

        [Command("Increment")]
        [Summary("This command is used for incrimenting the instance")]
        public async Task IncrimentInstanceCommand()
        {
            var sb = new StringBuilder();

            try
            {
                sb.AppendLine($"The current instance is {_instance.Instance}");

                await _instance.IncrimentInstance();

                sb.AppendLine($"The new instance is {_instance.Instance}");

                await ReplyAsync(sb.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync($"{nameof(IncrimentInstanceCommand)} has failed");
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using Elemonsters.Services;
using Elemonsters.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

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
        private ILockoutService _locker;
        private IInstanceTrackerService _instance;
        private IPartyService _partyService;
        private IBattleService _battleService;
        private DamageFactory _damageFactory;

        #endregion

        #region CTOR

        public CommandTesting(ILockoutService locker,
                              IInstanceTrackerService instance,
                              IPartyService partyService,
                              IBattleService battleService,
                              DamageFactory damageFactory)
        {
            _locker = locker;
            _instance = instance;
            _partyService = partyService;
            _battleService = battleService;
            _damageFactory = damageFactory;
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
                var instance = await _instance.GetInstance();

                if (userCheck == false)
                {
                    sb.AppendLine($"Test 1: {context.User.Mention} was not locked");
                }
                else
                {
                    sb.AppendLine($"Test 1: {context.User.Mention} was locked");
                }

                var userInstanceCheck = await _locker.CheckActivityLock(context.User, _activity, instance);

                if (userInstanceCheck == false)
                {
                    sb.AppendLine($"Test 2: {context.User.Mention} was not performing {_activity} for the instance {instance}");
                }
                else
                {
                    sb.AppendLine($"Test 2: {context.User.Mention} was performing {_activity} specified for instance {instance}");
                }

                var userLockCheck = await _locker.LockUser(context.User, _activity, instance);

                if (userLockCheck == false)
                {
                    sb.AppendLine($"Test 3: Unable to lock {context.User.Mention} with the instance {instance} and activity {_activity}");
                }
                else
                {
                    sb.AppendLine($"Test 3: {context.User.Mention} was locked with the instance {instance} and activity {_activity}");
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
            var instance = await _instance.GetInstance();

            try
            {
                sb.AppendLine($"The current instance is {instance}");

                await _instance.IncrimentInstance();

                sb.AppendLine($"The new instance is {instance}");

                await ReplyAsync(sb.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync($"{nameof(IncrimentInstanceCommand)} has failed");
            }
        }

        [Command("Battle", RunMode = RunMode.Async)]
        [Summary("This command is used for testing battle flow")]
        public async Task BattleTestCommand()
        {
            try
            {
                var context = Context;

                var players = new List<IUser>();

                players.Add(context.User);

                var battleContainer = new BattleContainer
                {
                    Players = players,
                    Instance = await _instance.GetInstance(),
                    Creatures = new List<CreatureBase>(),
                };

                await _battleService.BeginBattle(context, battleContainer);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}

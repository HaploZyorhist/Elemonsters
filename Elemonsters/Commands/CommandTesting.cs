using System.Text;
using Discord;
using Discord.Commands;
using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using Elemonsters.Models.Locker.Requests;
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
        private ILockoutService _locker;
        private IPartyService _partyService;
        private IBattleService _battleService;

        #endregion

        #region CTOR

        public CommandTesting(ILockoutService locker,
                              IPartyService partyService,
                              IBattleService battleService)
        {
            _locker = locker;
            _partyService = partyService;
            _battleService = battleService;
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
                var request = new UnlockUserRequest
                {
                    User = context.User.Id
                };
                var unlockResult = await _locker.UnlockUser(request);

                if (unlockResult.IsLocked)
                {
                    throw new Exception($"{Context.User.Mention} was not unlocked");
                }

                await ReplyAsync($"{context.User.Mention} was unlocked");
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
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
                var request = new LockUserRequest
                {
                    User = context.User.Id,
                    Activity = _activity,
                    Instance = Guid.NewGuid()
                };

                var result = await _locker.LockUser(request);

                if (!result.IsLocked)
                {
                    sb.AppendLine($"Test 1: {context.User.Mention} was not locked");
                }
                else
                {
                    sb.AppendLine($"Test 1: {context.User.Mention} was locked");
                }

                var compareRequest = new CompareLockRequest
                {
                    User = context.User.Id,
                    Activity = _activity,
                    Instance = request.Instance
                };

                var compareResult = await _locker.CompareLock(compareRequest);

                if (!compareResult.LockMatch)
                {
                    sb.AppendLine($"Test 2: {context.User.Mention} was not performing {_activity} for the instance {compareRequest.Instance}");
                }
                else
                {
                    sb.AppendLine($"Test 2: {context.User.Mention} was performing {_activity} specified for instance {compareRequest.Instance}");
                }

                await ReplyAsync(sb.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync($"Locking test has failed");
            }
        }

        [Command("Battle", RunMode = RunMode.Async)]
        [Summary("This command is used for testing battle flow")]
        public async Task BattleTestCommand()
        {
            var context = Context;

            var instance = Guid.NewGuid();

            try
            {
                var players = new List<IUser>();
                players.Add(context.User);

                foreach (var player in players)
                {
                    var checkLockRequest = new CheckLockRequest
                    {
                        User = player.Id
                    };

                    var lockedCheckResult = await _locker.CheckLock(checkLockRequest);

                    if (lockedCheckResult.IsLocked)
                    {
                        throw new Exception(
                            $"{player.Mention} is currently locked in another activity. Starting battle failed.");
                    }

                    var lockPlayerRequest = new LockUserRequest
                    {
                        User = player.Id,
                        Activity = ActivityEnum.Battle,
                        Instance = instance
                    };

                    await _locker.LockUser(lockPlayerRequest);
                }

                var battleContainer = new BattleContainer
                {
                    Players = players,
                    Instance = instance,
                    Creatures = new List<CreatureBase>(),
                    Context = context
                };

                await _battleService.BeginBattle(context, battleContainer);
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        #endregion
    }
}

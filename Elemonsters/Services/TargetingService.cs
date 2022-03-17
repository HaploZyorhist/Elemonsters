using System.Net;
using System.Text;
using System.Xml.Serialization;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Chat;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for handling targeting
    /// </summary>
    public class TargetingService : ITargetingService
    {
        private readonly IChatService _chatService;

        /// <summary>
        /// constructor for targeting service
        /// </summary>
        /// <param name="chatService">chat service for interacting with discord</param>
        public TargetingService(IChatService chatService)
        {
            _chatService = chatService;
        }

        /// <inheritdoc />
        public async Task<List<CreatureBase>> GetTargets(GetTargetsRequest request)
        {
            try
            {
                // temp ai
                if (request.MyTurn == 0)
                {
                    return request.Container.Creatures.Where(x => x.CreatureID != request.MyTurn).ToList();
                }

                switch (request.Rules.Rule)
                {
                    case TargetingRulesEnum.Standard:
                        return await GetStandardTargets(request);

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<CreatureBase>> GetStandardTargets(GetTargetsRequest request)
        {
            try
            {
                var me = request.Container.Creatures.Where(x => x.CreatureID == request.MyTurn).FirstOrDefault();

                List<CreatureBase> availableTargets = request.Container.Creatures
                    .Where(x => x.Position == PositionEnum.Melee &&
                                x.User != me.User)
                    .ToList();

                if (availableTargets.Count == 0)
                {
                    availableTargets.AddRange(request.Container.Creatures
                        .Where(x => x.Position == PositionEnum.Ranged &&
                                    x.User != me.User)
                        .ToList());
                }

                if (availableTargets.Count == 0)
                {
                    availableTargets.AddRange(request.Container.Creatures
                        .Where(x => x.Position == PositionEnum.Auxillary &&
                                    x.User != me.User)
                        .ToList());
                }

                StringBuilder sb = new StringBuilder();

                var chosenTargets = new List<CreatureBase>();

                while (chosenTargets.Count < request.Rules.TotalTargets)
                {
                    sb.AppendLine($"<@{me.User}> please select one of the following targets.");

                    int i = 1;
                    availableTargets.ForEach(x => sb.AppendLine($"{i++}: Enemy {x.Name}"));

                    var playerResponseRequest = new PlayerResponseRequest
                    {
                        Message = sb.ToString(),
                        Context = request.Context,
                        User = me.User
                    };

                    var playerResponse = await _chatService.GetPlayerResponse(playerResponseRequest);

                    var choice = int.Parse(playerResponse);

                    chosenTargets.Add(availableTargets[choice - 1]);

                    if (request.Rules.UniqueTargets)
                    {
                        availableTargets.Remove(availableTargets[choice - 1]);
                    }

                    sb.Clear();
                }

                return chosenTargets;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

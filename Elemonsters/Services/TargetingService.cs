﻿using System.Text;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Chat;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;

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
        public async Task<List<ulong>> GetTargets(GetTargetsRequest request)
        {
            try
            {
                switch (request.Rules.Rule)
                {
                    case TargetingRulesEnum.Standard:
                        return await GetStandardTargets(request);

                    case TargetingRulesEnum.Self:
                        return await GetSelfTarget(request);

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// standard method of getting targets
        /// </summary>
        private async Task<List<ulong>> GetStandardTargets(GetTargetsRequest request)
        {
            try
            {
                // temp ai
                if (request.MyTurn == 0)
                {
                    var targets = new List<ulong>();
                    var aiTargets = request.Creatures.Where(x => x.CreatureID != request.MyTurn).Select(x => x.CreatureID).ToList();
                    targets.AddRange(aiTargets);
                    return targets;
                }

                var me = request.Creatures.Where(x => x.CreatureID == request.MyTurn).FirstOrDefault();

                List<CreatureBase> availableTargets = request.Creatures
                    .Where(x => x.Position == PositionEnum.Melee &&
                                x.User != me.User)
                    .ToList();

                if (availableTargets.Count == 0)
                {
                    availableTargets.AddRange(request.Creatures
                        .Where(x => x.Position == PositionEnum.Ranged &&
                                    x.User != me.User)
                        .ToList());
                }

                if (availableTargets.Count == 0)
                {
                    availableTargets.AddRange(request.Creatures
                        .Where(x => x.Position == PositionEnum.Auxillary &&
                                    x.User != me.User)
                        .ToList());
                }

                StringBuilder sb = new StringBuilder();

                var chosenTargets = new List<ulong>();

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

                    chosenTargets.Add(availableTargets[choice - 1].CreatureID);

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

        /// <summary>
        /// method for getting self as a target
        /// </summary>
        private async Task<List<ulong>> GetSelfTarget(GetTargetsRequest request)
        {
            try
            {
                List<ulong> targets = new List<ulong>();

                // temp ai
                if (request.MyTurn == 0)
                {
                    var aiTargets = request.Creatures.Where(x => x.CreatureID == request.MyTurn).Select(x => x.CreatureID).ToList();
                    targets.AddRange(aiTargets);
                    return targets;
                }

                targets.Add(request.MyTurn);

                return targets;
            }
            catch
            {
                return null;
            }
        }
    }
}

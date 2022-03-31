﻿using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for handling creatures
    /// </summary>
    public class CreatureService : ICreatureService
    {
        public IDBService _dbService;
        public StatFactory _statFactory;
        public readonly DamageFactory _damageFactory;

        /// <summary>
        /// ctor for adding creatures to the list of available creatures
        /// </summary>
        public CreatureService(IDBService dbService,
                               StatFactory statFactory,
                               DamageFactory damageFactory)
        {
            _dbService = dbService;
            _statFactory = statFactory;
            _damageFactory = damageFactory;
        }

        /// <inheritdoc />
        public async Task<CreatureBase> GetCreatureStats(StatsRequest creatureRequest)
        {
            try
            {
                var creature = await _dbService.GetCreatureBase("TestyBoi");

                var creatureStats = await _dbService.GetCreatureStats(creatureRequest.CreatureID);

                var updatedStats = await _statFactory.GenerateStats(creatureStats);

                var creatureElements = await _dbService.GetCreatureElements(creatureRequest.CreatureID);

                creature.Stats = updatedStats;
                creature.Elements = creatureElements;
                creature.CreatureID = creatureRequest.CreatureID;
                
                return creature;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

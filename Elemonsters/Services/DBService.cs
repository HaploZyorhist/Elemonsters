using Elemonsters.Services.Interfaces;
using Elemonsters.Assets.Creatures;
using Elemonsters.Assets.Creatures.PassiveAbilities;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Enums;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for interacting with the database
    /// </summary>
    public class DBService : IDBService
    {
        /// <inheritdoc />
        public async Task<List<StatsRequest>> GetPlayerParty(ulong playerId)
        {
            try
            {
                var creatureIDs = new List<StatsRequest>();

                var request = new StatsRequest();

                if (playerId == 947509644706869269)
                {
                    request.CreatureName = "Testy";
                    request.CreatureID = 0;
                }
                else
                {
                    request.CreatureName = "Testy";
                    request.CreatureID = 1;
                }
                creatureIDs.Add(request);

                return creatureIDs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<CreatureStats> GetCreatureStats(ulong creatureID)
        {
            try
            {
                CreatureStats stats = new CreatureStats();

                if (creatureID == 0)
                {
                    stats.MaxHealth = 1000;
                    stats.Health = stats.MaxHealth;
                    stats.MaxEnergy = 100;
                    stats.Energy = 0;
                    stats.Strength = 100;
                    stats.Defense = 100;
                    stats.Lethality = 10;
                    stats.Spirit = 50;
                    stats.Aura = 200;
                    stats.Sorcery = 10;
                    stats.CritChance = 50;
                    stats.CritModifier = 1.5;
                    stats.Dodge = 10;
                    stats.Tenacity = 10;
                    stats.Regeneration = 10;
                    stats.Speed = 9;
                }
                else
                {
                    stats.MaxHealth = 1000;
                    stats.Health = stats.MaxHealth;
                    stats.MaxEnergy = 100;
                    stats.Energy = 0;
                    stats.Strength = 100;
                    stats.Defense = 100;
                    stats.Lethality = 10;
                    stats.Spirit = 50;
                    stats.Aura = 200;
                    stats.Sorcery = 10;
                    stats.CritChance = 50;
                    stats.CritModifier = 5;
                    stats.Dodge = 10;
                    stats.Tenacity = 10;
                    stats.Regeneration = 10;
                    stats.Speed = 10;
                }

                return stats;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<CreatureBase> GetCreatureBase(string creatureClass)
        {
            try
            {
                var type = typeof(CreatureBase).Assembly.GetTypes().Single(t => t.Name == creatureClass);

                var creature = (CreatureBase)Activator.CreateInstance(type);

                return creature;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

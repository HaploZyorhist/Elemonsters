using Elemonsters.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat;
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

                if (playerId == 0)
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
                    stats.Energy = stats.MaxEnergy;
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
                    stats.Speed = 10;
                    stats.Vamp = 10;
                    stats.Drain = 10;
                }
                else
                {
                    stats.MaxHealth = 1000;
                    stats.Health = stats.MaxHealth;
                    stats.MaxEnergy = 100;
                    stats.Energy = stats.MaxEnergy;
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
                    stats.Speed = 10;
                    stats.Vamp = 10;
                    stats.Drain = 10;
                }

                return stats;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<CreatureBase> GetCreatureBase(string creatureName)
        {
            try
            {
                CreatureBase creature = new CreatureBase();

                return creature;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<CreatureElements> GetCreatureElements(ulong creatureID)
        {
            try
            {
                CreatureElements elements = new CreatureElements();

                if (creatureID == 0)
                {
                    elements.PhysicalElement = PhysicalElement.Fire;
                    elements.PhysicalValue = 100;
                    elements.MagicElement = MagicElement.Wind;
                    elements.MagicValue = 100;
                }
                else
                {
                    elements.PhysicalElement = PhysicalElement.Water;
                    elements.PhysicalValue = 100;
                    elements.MagicElement = MagicElement.Earth;
                    elements.MagicValue = 100;
                }

                return elements;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

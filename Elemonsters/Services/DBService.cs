using Elemonsters.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Assets.Creatures.ActiveAbilities;
using Elemonsters.Assets.Creatures.PassiveAbilities;
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
                    stats.Speed = 10;
                    stats.Vamp = 10;
                    stats.Drain = 10;
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
        public async Task<CreatureBase> GetCreatureBase(CreatureBase creature, StatsRequest creatureRequest)
        {
            try
            {
                creature.Abilities.Add(new Ability
                {
                    Name = "Basic Attack",
                    AbilityLevel = 1,
                    AbilityType = AbilityTypes.BasicAttack
                });

                creature.Abilities.Add(new Ability
                {
                    Name = "Test Passive",
                    AbilityLevel = 1,
                    AbilityType = AbilityTypes.Passive
                });

                creature.Abilities.Add(new Ability
                {
                    Name = "Test Shielding",
                    AbilityLevel = 1,
                    AbilityType = AbilityTypes.FirstAbility
                });

                var type = typeof(Ability).Assembly.GetTypes().Single(t => t.Name == "BasicAttackAbility");

                var basicAttackAbility = creature.Abilities
                    .Where(x => string.Equals(x.Name, "Basic Attack", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                basicAttackAbility.ActiveAbility = (ActiveAbility)Activator.CreateInstance(type);

                type = typeof(Ability).Assembly.GetTypes().Single(t => t.Name == "TestPassive");

                var passiveAbility = creature.Abilities
                    .Where(x => string.Equals(x.Name, "Test Passive", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                passiveAbility.PassiveAbility = (PassiveAbility)Activator.CreateInstance(type);
                passiveAbility.PassiveAbility.TriggerConditions = TriggerConditions.OnHit;

                type = typeof(Ability).Assembly.GetTypes().Single(t => t.Name == "GenerateShieldAbility");

                var firstAbility = creature.Abilities
                    .Where(x => string.Equals(x.Name, "Test Shielding", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                firstAbility.ActiveAbility = (ActiveAbility)Activator.CreateInstance(type);

                creature.CreatureID = creatureRequest.CreatureID;

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

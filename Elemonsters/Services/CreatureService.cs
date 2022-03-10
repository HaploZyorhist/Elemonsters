using Elemonsters.Assets.Creatures;
using Elemonsters.Assets.Creatures.CreatureBases;
using Elemonsters.Models.Enums;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for handling creatures
    /// </summary>
    public class CreatureService : ICreatureService
    {
        public IDBService _serv;

        /// <summary>
        /// ctor for adding creatures to the list of available creatures
        /// </summary>
        public CreatureService(IDBService serv)
        {
            _serv = serv;
        }

        /// <inheritdoc />
        public async Task<CreatureBase> GetCreatureStats(ulong cretureID)
        {
            try
            {
                //TODO Get the stats from the database using creature id
                //TODO Get upgrades from the database using creature id
                //TODO use the stat factory to get the stats for the creature

                if (playerID == 0)
                {
                    creature.User = playerID;
                    creature.Stats.MaxHealth = 1000;
                    creature.Stats.Health = creature.Stats.MaxHealth;
                    creature.Stats.MaxEnergy = 100;
                    creature.Stats.Energy = creature.Stats.MaxEnergy;
                    creature.Stats.Strength = 100;
                    creature.Stats.Defense = 100;
                    creature.Stats.Lethality = 10;
                    creature.Stats.Spirit = 50;
                    creature.Stats.Aura = 200;
                    creature.Stats.Sorcery = 10;
                    creature.Stats.CritChance = 50;
                    creature.Stats.CritModifier = 150;
                    creature.Stats.Dodge = 10;
                    creature.Stats.Tenacity = 10;
                    creature.Stats.Regeneration = 10;
                    creature.Stats.Speed = 10;
                    creature.Stats.Vamp = 10;
                    creature.Stats.Drain = 10;
                    creature.Elements.PhysicalElement = PhysicalElement.Wood;
                    creature.Elements.MagicElement = MagicElement.Electric;
                    creature.Elements.PhysicalValue = 100;
                    creature.Elements.RangedValue = 100;
                    creature.Level = 1;
                    creature.Rank = 1;
                }
                else
                {
                    creature.User = playerID;
                    creature.Stats.MaxHealth = 1000;
                    creature.Stats.Health = creature.Stats.MaxHealth;
                    creature.Stats.MaxEnergy = 100;
                    creature.Stats.Energy = creature.Stats.MaxEnergy;
                    creature.Stats.Strength = 100;
                    creature.Stats.Defense = 100;
                    creature.Stats.Lethality = 10;
                    creature.Stats.Spirit = 50;
                    creature.Stats.Aura = 200;
                    creature.Stats.Sorcery = 10;
                    creature.Stats.CritChance = 50;
                    creature.Stats.CritModifier = 150;
                    creature.Stats.Dodge = 10;
                    creature.Stats.Tenacity = 10;
                    creature.Stats.Regeneration = 10;
                    creature.Stats.Speed = 10;
                    creature.Stats.Vamp = 10;
                    creature.Stats.Drain = 10;
                    creature.Elements.PhysicalElement = PhysicalElement.Fire;
                    creature.Elements.MagicElement = MagicElement.Earth;
                    creature.Elements.PhysicalValue = 100;
                    creature.Elements.RangedValue = 100;
                    creature.Level = 1;
                    creature.Rank = 1;
                }

                return creature;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

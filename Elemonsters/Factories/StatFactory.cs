using Elemonsters.Assets.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Factories
{
    /// <summary>
    /// factory for generating stats for a creature
    /// </summary>
    public class StatFactory
    {
        /// <summary>
        /// method for generating stats for an instance from the base
        /// </summary>
        public CreatureStats GenerateStats (CreatureParent creature)
        {
            try
            {
                var newStats = new CreatureStats ();

                newStats.Strength = creature.Stats.Strength;
                newStats.Defense = creature.Stats.Defense;
                newStats.Lethality = creature.Stats.Lethality;
                newStats.Aura = creature.Stats.Aura;
                newStats.Spirit = creature.Stats.Spirit;
                newStats.Sorcery = creature.Stats.Sorcery;
                newStats.Health = creature.Stats.Health;
                newStats.Energy = creature.Stats.Energy;
                newStats.MaxHealth = creature.Stats.MaxHealth;
                newStats.MaxEnergy = creature.Stats.MaxEnergy;
                newStats.Speed = creature.Stats.Speed;
                newStats.Vamp = creature.Stats.Vamp;
                newStats.Drain = creature.Stats.Drain;
                newStats.CritChance = creature.Stats.CritChance;
                newStats.CritModifier = creature.Stats.CritModifier;
                newStats.Regeneration = creature.Stats.Regeneration;
                newStats.Dodge = creature.Stats.Dodge;
                newStats.Tenacity = creature.Stats.Tenacity;

                return newStats;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

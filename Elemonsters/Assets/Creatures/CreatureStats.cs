using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Assets.Creatures
{
    /// <summary>
    /// class containing creature stats
    /// </summary>
    public class CreatureStats
    {
        /// <summary>
        /// used for calculating the effectiveness of physical abilities
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// used for calculating the resistance to physical abilities
        /// </summary>
        public int Defense { get; set; }

        /// <summary>
        /// used for calculating amount of defense to ignore
        /// </summary>
        public int Lethality { get; set; }

        /// <summary>
        /// used for calculating the effectiveness of elemental abilities
        /// </summary>
        public int Spirit { get; set; }

        /// <summary>
        /// used for calculating the resistance to elemental abilities
        /// </summary>
        public int Aura { get; set; }

        /// <summary>
        /// used for calculating the amount of aura to ignore
        /// </summary>
        public int Sorcery { get; set; }

        /// <summary>
        /// creature's current health
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// maximum health value for the creature
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// creature's current energy value
        /// </summary>
        public int Energy { get; set; }

        /// <summary>
        /// maximum energy value for the creature
        /// </summary>
        public int MaxEnergy { get; set; }

        /// <summary>
        /// creature's speed
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// used for reducing the chance of a cc effect hitting the target
        /// </summary>
        public int Tenacity { get; set; }

        /// <summary>
        /// used for reducing the chance of a physical effect hitting the target
        /// </summary>
        public int Dodge { get; set; }

        /// <summary>
        /// used for causing physical attacks to deal extra damage
        /// </summary>
        public int CritChance { get; set; }

        /// <summary>
        /// how much physical damage is multiplied by when dealing crit damage
        /// </summary>
        public int CritModifier { get; set; }

        /// <summary>
        /// how much energy the creature gains per turn
        /// </summary>
        public int Regeneration { get; set; }

        /// <summary>
        /// how much health the creature gains when dealing physical damage
        /// </summary>
        public int Vamp { get; set; }

        /// <summary>
        /// how much health the creature gains when dealing elemental damage
        /// </summary>
        public int Drain { get; set; }
    }
}

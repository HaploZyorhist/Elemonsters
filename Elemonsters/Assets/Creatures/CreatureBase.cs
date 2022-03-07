using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Assets.Creatures
{
    /// <summary>
    /// parent class for creatures
    /// </summary>
    public abstract class CreatureBase
    {
        /// <summary>
        /// creature's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// user of the creature
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// the stats of the creature
        /// </summary>
        public CreatureStats Stats { get; set; }

        /// <summary>
        /// the element types and values of the creature
        /// </summary>
        public CreatureElements Elements { get; set; }
        
        /// <summary>
        /// creature's level
        /// </summary>
        public int Level { get; set; }

        //TODO This needs to be an enum
        /// <summary>
        /// the current upgrade of the creature
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// the passive ability of the creature
        /// </summary>
        public abstract Task Passive();

        /// <summary>
        /// the creature's first main ability
        /// </summary>
        /// <returns></returns>
        public abstract Task Ability1();

        /// <summary>
        /// the creature's second main ability
        /// </summary>
        /// <returns></returns>
        public abstract Task Ability2();

        /// <summary>
        /// the creature's third main ability
        /// </summary>
        /// <returns></returns>
        public abstract Task Ability3();
    }
}

using Elemonsters.Models.Enums;
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
    public class CreatureBase
    {
        #region Fields

        /// <summary>
        /// creature's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// user of the creature
        /// </summary>
        public ulong User { get; set; }

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
        /// the stats of the creature
        /// </summary>
        public CreatureStats Stats { get; set; }

        /// <summary>
        /// the element types and values of the creature
        /// </summary>
        public CreatureElements Elements { get; set; }

        public Ability PassiveAbility { get; set; }

        public Ability AutoAttack { get; set; }

        public Ability Ability1 { get; set; }

        public Ability Ability2 { get; set; }

        public Ability Ability3 { get; set; }

        #endregion

        #region CTOR

        public CreatureBase()
        {
            Name = "Testy";
            User = 0;
            Level = 1;
            Rank = 1;
            Stats = new CreatureStats
            {
                Strength =  100,
                Defense = 100,
                Lethality = 10,
                Spirit = 100,
                Aura = 100,
                Sorcery = 10,
                CritChance = 100,
                CritModifier = 150,
                Dodge = 100,
                Tenacity = 100,
                Drain = 100,
                Vamp = 100,
                MaxHealth = 1000,
                Health = 1000,
                MaxEnergy = 100,
                Energy = 100,
                Regeneration = 100,
                Speed = 10,
            };
            Elements = new CreatureElements
            {
                PhysicalElement = PhysicalElement.Fire,
                PhysicalValue = 100,
                MagicElement = MagicElement.Wind,
                MagicValue = 100,
            };
            PassiveAbility = new Ability();
            AutoAttack = new Ability();
            Ability1 = new Ability();
            Ability2 = new Ability();
            Ability3 = new Ability();
        }

        #endregion
    }
}

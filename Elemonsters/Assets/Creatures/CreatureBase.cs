﻿using Elemonsters.Models.Enums;
using Elemonsters.Assets.StatusEffects;

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
        /// bool indicating if the creature is the leader of the party
        /// </summary>
        public bool IsLeader { get; set; }

        /// <summary>
        /// user of the creature
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// id for the creature
        /// </summary>
        public ulong CreatureID { get; set; }

        /// <summary>
        /// creature's level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// stat for tracking turn order
        /// </summary>
        public int ActionPoints { get; set; }

        /// <summary>
        /// The position the creature is currently occupying
        /// </summary>
        public PositionEnum Position { get; set; }

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

        /// <summary>
        /// The creature's abilities
        /// </summary>
        public List<Ability> Abilities { get; set; } = new List<Ability>();

        /// <summary>
        /// list of buffs and debuffs that the character currently has applied
        /// </summary>
        public List<BuffDebuff> Statuses { get; set; } = new List<BuffDebuff>();

        #endregion

        #region CTOR

        /// <summary>
        /// base constructor for the creature object
        /// </summary>
        public CreatureBase()
        {
            Name = "Testy";
            IsLeader = false;
            CreatureID = 0;
            User = 0;
            Level = 1;
            Rank = 1;
            ActionPoints = 0;
            Position = PositionEnum.Melee;
            Stats = new CreatureStats
            {
                Strength = 100,
                Defense = 100,
                Lethality = 10,
                Spirit = 100,
                Aura = 100,
                Sorcery = 10,
                CritChance = 100,
                CritModifier = 150,
                Dodge = 100,
                Tenacity = 100,
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
                RangedElement = MagicElement.Wind,
                RangedValue = 100,
            };
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// causes the creature to gain action points based on their speed
        /// </summary>
        public async Task Tick()
        {
            try
            {
                ActionPoints += Stats.Speed;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// method for increasing energy level of a creature
        /// </summary>
        /// <param name="bonus">amount of bonus energy gained</param>
        /// <param name="modifier">multiplier for regen value</param>
        public async Task<int> Gain(int bonus, double modifier)
        {
            try
            {
                var currentEnergy = Stats.Energy;

                var energyGain = Stats.Regeneration * modifier;

                Stats.Energy += bonus + (int)energyGain;

                if (Stats.Energy > Stats.MaxEnergy)
                {
                    Stats.Energy = Stats.MaxEnergy;
                }

                var totalGain = Stats.Energy - currentEnergy;

                return totalGain;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion
    }
}

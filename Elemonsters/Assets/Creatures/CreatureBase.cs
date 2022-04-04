using Elemonsters.Models.Enums;
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

        /// <summary>
        /// the current upgrade of the creature
        /// </summary>
        public RankEnum Rank { get; set; } = RankEnum.Bronze;

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

        #region Turn Progress Methods

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

        #region GetStats

        /// <summary>
        /// method for calculating stats after buff/debuff effects
        /// </summary>
        /// <param name="stat">enum for checking for flat buffs</param>
        /// <param name="pStat">enum for checking for percent buffs</param>
        /// <param name="currentStat">the current value of the stat</param>
        /// <returns></returns>
        public virtual async Task<int> CalculateStat(StatEffectedEnum stat, StatEffectedEnum pStat, int currentStat)
        {
            int result;

            var flatBuff = Statuses.Where(x => x.IsBuff &&
                                               x.Stat == stat)
                ?.OrderByDescending(x => x.Value)
                ?.FirstOrDefault()
                ?.Value;

            flatBuff = flatBuff == null ?
                0 :
                flatBuff;

            var percentBuff = Statuses.Where(x => x.IsBuff &&
                                                  x.Stat == stat)
                ?.OrderByDescending(x => x.Value)
                ?.FirstOrDefault()
                ?.Value;

            percentBuff = percentBuff == null ?
                0 :
                percentBuff;

            var flatDebuff = Statuses.Where(x => !x.IsBuff &&
                                                 x.Stat == pStat)
                ?.OrderByDescending(x => x.Value)
                ?.FirstOrDefault()
                ?.Value;

            flatDebuff = flatDebuff == null ?
                0 :
                flatDebuff;

            var percentDebuff = Statuses.Where(x => !x.IsBuff &&
                                                    x.Stat == pStat)
                ?.OrderByDescending(x => x.Value)
                ?.FirstOrDefault()
                ?.Value;

            percentDebuff = percentDebuff == null ?
                0 :
                percentDebuff;

            result = currentStat + (int)flatBuff - (int)flatDebuff;

            result *= (100 + (int)percentBuff) / (100 + (int)percentDebuff);

            return result;
        }

        /// <summary>
        /// method for getting current max health
        /// </summary>
        /// <returns>int indicating current max health</returns>
        public virtual async Task<int> GetCurrentMaxHealth()
        {
            var maxHealth = Stats.MaxHealth;

            var stat = StatEffectedEnum.MaxHealth;
            var pStat = StatEffectedEnum.PMaxHealth;

            return await CalculateStat(stat, pStat, maxHealth);
        }

        /// <summary>
        /// method for getting current max energy
        /// </summary>
        /// <returns>int indicating current max energy</returns>
        public virtual async Task<int> GetCurrentMaxEnergy()
        {
            var maxEnergy = Stats.MaxEnergy;

            var stat = StatEffectedEnum.MaxEnergy;
            var pStat = StatEffectedEnum.PMaxEnergy;

            return await CalculateStat(stat, pStat, maxEnergy);
        }

        /// <summary>
        /// method for getting current strength
        /// </summary>
        /// <returns>int indicating current strength</returns>
        public virtual async Task<int> GetCurrentStrength()
        {
            var strength = Stats.Strength;

            var stat = StatEffectedEnum.Strength;
            var pStat = StatEffectedEnum.PStrength;

            return await CalculateStat(stat, pStat, strength);
        }

        /// <summary>
        /// method for getting current defense
        /// </summary>
        /// <returns>int indicating current defense</returns>
        public virtual async Task<int> GetCurrentDefense()
        {
            var defense = Stats.Defense;

            var stat = StatEffectedEnum.Defense;
            var pStat = StatEffectedEnum.PDefense;

            return await CalculateStat(stat, pStat, defense);
        }

        /// <summary>
        /// method for getting current lethality
        /// </summary>
        /// <returns>int indicating current lethality</returns>
        public virtual async Task<int> GetCurrentLethality()
        {
            var lethality = Stats.Lethality;

            var stat = StatEffectedEnum.Lethality;
            var pStat = StatEffectedEnum.PLethality;

            return await CalculateStat(stat, pStat, lethality);
        }

        /// <summary>
        /// method for getting current spirit
        /// </summary>
        /// <returns>int indicating current spirit</returns>
        public virtual async Task<int> GetCurrentSpirit()
        {
            var spirit = Stats.Spirit;

            var stat = StatEffectedEnum.Spirit;
            var pStat = StatEffectedEnum.PSpirit;

            return await CalculateStat(stat, pStat, spirit);
        }

        /// <summary>
        /// method for getting current aura
        /// </summary>
        /// <returns>int indicating current aura</returns>
        public virtual async Task<int> GetCurrentAura()
        {
            var aura = Stats.Aura;

            var stat = StatEffectedEnum.Aura;
            var pStat = StatEffectedEnum.PAura;

            return await CalculateStat(stat, pStat, aura);
        }

        /// <summary>
        /// method for getting current sorcery
        /// </summary>
        /// <returns>int indicating current sorcery</returns>
        public virtual async Task<int> GetCurrentSorcery()
        {
            var sorcery = Stats.Sorcery;

            var stat = StatEffectedEnum.Sorcery;
            var pStat = StatEffectedEnum.PSorcery;

            return await CalculateStat(stat, pStat, sorcery);
        }

        #endregion
    }
}

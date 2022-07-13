using System.Text;
using Elemonsters.Models.Enums;
using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.DamageFactory.Results;

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
        /// the species the creature belongs to
        /// </summary>
        public string Species { get; set; }

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
        public Dictionary<ElementEnum, int> Elements { get; set; } = new Dictionary<ElementEnum, int>();

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

        #region Combat Methods

        /// <summary>
        /// method for dealing damage to targets
        /// </summary>
        /// <param name="damageRequests">request object dictating how to deal damage to target</param>
        public async Task<StringBuilder> HandleDamage(DamageFactoryResults damage)
        {
            var messages = new StringBuilder();

            try
            {
                var totalDamage = damage.ElementalDamage + damage.PhysicalDamage + damage.TrueDamage;

                var elementalShields = Statuses
                    .Where(x =>
                        x.EffectType == EffectTypesEnum.ElementalShield)
                    .OrderBy(x => x.Duration)
                    .ToList();

                if (elementalShields != null && elementalShields.Count > 0)
                {
                    var totalElementalShield = elementalShields
                        .Select(x => x.Value)
                        .Sum();

                    messages
                        .AppendLine($"<@{User}>'s {Name} currently has {totalElementalShield} Elemental Shield");
                }

                var physicalShields = Statuses
                    .Where(x =>
                        x.EffectType == EffectTypesEnum.PhysicalShield)
                    .OrderBy(x => x.Duration)
                    .ToList();

                if (physicalShields != null && physicalShields.Count > 0)
                {
                    var totalPhysicalShield = physicalShields
                        .Select(x => x.Value)
                        .Sum();

                    messages
                        .AppendLine($"<@{User}>'s {Name} currently has {totalPhysicalShield} Physical Shield");
                }

                var generalShields = Statuses
                    .Where(x =>
                        x.EffectType == EffectTypesEnum.GeneralShield)
                    .OrderBy(x => x.Duration)
                    .ToList();

                if (generalShields != null && generalShields.Count > 0)
                {
                    var totalGeneralShield = generalShields
                        .Select(x => x.Value)
                        .Sum();

                    messages
                        .AppendLine($"<@{User}>'s {Name} currently has {totalGeneralShield} General Shield");
                }

                int remainingDamage = 0;

                int remainingElementalDamage = damage.ElementalDamage;
                int remainingPhysicalDamage = damage.PhysicalDamage;

                while (elementalShields != null &&
                       elementalShields.Count > 0 &&
                       remainingElementalDamage > 0)
                {
                    var currentShield = elementalShields.First();

                    if (currentShield.Value > remainingElementalDamage)
                    {
                        currentShield.Value -= remainingElementalDamage;
                        remainingElementalDamage = 0;
                    }
                    else
                    {
                        remainingElementalDamage -= currentShield.Value;
                        elementalShields.Remove(currentShield);
                    }
                }

                if (damage.ElementalDamage != remainingElementalDamage)
                {
                    messages.AppendLine(
                        $"<@{User}>'s {Name}'s Elemental Shield has blocked {damage.ElementalDamage - remainingElementalDamage} damage");
                }

                remainingDamage = remainingElementalDamage;

                while (physicalShields != null &&
                       physicalShields.Count > 0 &&
                       remainingPhysicalDamage > 0)
                {
                    var currentShield = physicalShields.First();

                    if (currentShield.Value > remainingPhysicalDamage)
                    {
                        currentShield.Value -= remainingPhysicalDamage;
                        remainingPhysicalDamage = 0;
                    }
                    else
                    {
                        remainingPhysicalDamage -= currentShield.Value;
                        physicalShields.Remove(currentShield);
                    }
                }

                if (damage.PhysicalDamage != remainingPhysicalDamage)
                {
                    messages.AppendLine(
                        $"<@{User}>'s {Name}'s Physical Shield has blocked {damage.PhysicalDamage - remainingPhysicalDamage} damage");
                }

                remainingDamage += remainingPhysicalDamage + damage.TrueDamage;
                var generalDamage = remainingDamage;

                while (generalShields != null &&
                       generalShields.Count > 0 &&
                       remainingDamage > 0)
                {
                    var currentShield = generalShields.First();

                    if (currentShield.Value > remainingDamage)
                    {
                        currentShield.Value -= remainingDamage;
                        remainingDamage = 0;
                    }
                    else
                    {
                        remainingDamage -= currentShield.Value;
                        generalShields.Remove(currentShield);
                    }
                }

                if (generalDamage != remainingDamage)
                {
                    messages.AppendLine(
                        $"<@{User}>'s {Name}'s General Shield has blocked {generalDamage - remainingDamage} damage");
                }

                if (remainingDamage > 0)
                {
                    Stats.Health -= remainingDamage;
                }

                messages.
                    AppendLine($"<@{User}>'s {Name} took {totalDamage} total damage, {totalDamage - remainingDamage} was blocked by shields.  " +
                               $"{remainingDamage} was dealt to their health");

                messages
                    .AppendLine($"{remainingElementalDamage} was Elemental Damage, {remainingPhysicalDamage} was Physical Damage, {damage.TrueDamage} was True Damage");

                if (Stats.Health <= 0)
                {
                    Stats.Health = 0;
                    messages.AppendLine($"<@{User}>'s {Name} has died");
                }

                messages.AppendLine(
                    $"<@{User}>'s {Name} has {Stats.Health} remaining health");

                Statuses.RemoveAll(x => (x.EffectType == EffectTypesEnum.ElementalShield ||
                                            x.EffectType == EffectTypesEnum.PhysicalShield ||
                                            x.EffectType == EffectTypesEnum.GeneralShield) && x.Value <= 0);

                return messages;
            }
            catch (Exception ex)
            {
                return null;
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

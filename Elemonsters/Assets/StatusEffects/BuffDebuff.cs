using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.StatusEffects
{
    /// <summary>
    /// object for handling a buff/debuff that is to be applied to a character
    /// </summary>
    public class BuffDebuff
    {
        /// <summary>
        /// name of the buff/debuff
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// indicates if the status effect is a buff or debuff
        /// </summary>
        public bool IsBuff { get; set; } = false;

        /// <summary>
        /// indicates how many turns the buff/debuff will last
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// indicates how many stacks of the buff/debuff are on the creature
        /// </summary>
        public int Stacks { get; set; }

        /// <summary>
        /// the level of the ability that has kicked off the buff/debuff
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// used for determining how effective the buff/debuff is
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// the type of buff/debuff
        /// </summary>
        public EffectTypesEnum EffectType { get; set; } = EffectTypesEnum.None;

        /// <summary>
        /// indicates how and when the buff/debuff can be activated
        /// </summary>
        public TriggerConditionsEnum TriggerConditions { get; set; } = TriggerConditionsEnum.None;

        /// <summary>
        /// creatures who can activate the effect
        /// </summary>
        public List<ulong> ActivatingCreatures { get; set; } = new List<ulong>();

        /// <summary>
        /// if the buff/debuff effects a stat, indicates what stat is effected
        /// </summary>
        public StatEffectedEnum Stat { get; set; } = StatEffectedEnum.None;

        public virtual async Task<StatusEffectResult> ActivateEffect(ActivateStatusEffectRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual async Task ReduceDuration(ReduceDurationRequest request)
        {
            Duration -= 1;
        }
    }
}

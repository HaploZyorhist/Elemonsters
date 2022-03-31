   using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing data for requesting damage information
    /// </summary>
    public class DamageRequest
    {
        /// <summary>
        /// type of attack being performed
        /// </summary>
        public AttackTypeEnum AttackType { get; set; } = AttackTypeEnum.None;

        /// <summary>
        /// type of effects that the ability will trigger
        /// </summary>
        public TriggerConditionsEnum TriggerCondition { get; set; } = TriggerConditionsEnum.None;

        /// <summary>
        /// object containing all of the details of the battle
        /// </summary>
        public BattleContainer Container { get; set; } = new BattleContainer();

        /// <summary>
        /// how much damage is being dealt
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// how much defense value is being ignored by the attack
        /// </summary>
        public int Penetration { get; set; }

        /// <summary>
        /// id of the creature taking the turn
        /// </summary>
        public ulong ActiveCreature { get; set; }

        /// <summary>
        /// target that the damage is being dealt to
        /// </summary>
        public ulong Target { get; set; }
    }
}

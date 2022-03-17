using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Factory.Requests
{
    /// <summary>
    /// object containing data for the Damage Factory to process damage
    /// </summary>
    public class DamageFactoryRequest
    {
        /// <summary>
        /// the creature initiating the attack
        /// </summary>
        public CreatureBase Attacker { get; set; } = new CreatureBase();

        /// <summary>
        /// creature being hit
        /// </summary>
        public CreatureBase Target { get; set; } = new CreatureBase();

        /// <summary>
        /// type of attack being performed
        /// </summary>
        public AttackTypeEnum AttackType { get; set; } = AttackTypeEnum.None;

        /// <summary>
        /// amount of damage being dealt
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// how much defense the attack ignores
        /// </summary>
        public int Penetration { get; set; }
    }
}

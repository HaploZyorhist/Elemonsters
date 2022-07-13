using Elemonsters.Models.Enums;

namespace Elemonsters.Models.DamageFactory.Requests
{
    public class DamageFactoryData
    {
        /// <summary>
        /// type of attack being performed
        /// </summary>
        public AttackTypeEnum AttackType { get; set; } = AttackTypeEnum.None;

        /// <summary>
        /// how much damage is being dealt
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// how much defense value is being ignored by the attack
        /// </summary>
        public int Penetration { get; set; }
    }
}

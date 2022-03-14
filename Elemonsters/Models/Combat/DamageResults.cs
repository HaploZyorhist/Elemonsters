using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat
{
    /// <summary>
    /// Object for returning results of an ability
    /// </summary>
    public class DamageResults
    {
        /// <summary>
        /// type of attack that was performed
        /// </summary>
        public AttackTypeEnum AttackType { get; set; }

        /// <summary>
        /// effect type that gets triggered as a result of the ability
        /// </summary>
        public TriggerConditions Trigger { get; set; } = TriggerConditions.None;

        /// <summary>
        /// how much damage the ability did
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// target of the ability
        /// </summary>
        public CreatureBase Target { get; set; }

        /// <summary>
        /// messages from the ability
        /// </summary>
        public StringBuilder SB { get; set; } = new StringBuilder();
    }
}

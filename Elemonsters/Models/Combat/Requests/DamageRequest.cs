using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing data for requesting damage information
    /// </summary>
    public class DamageRequest
    {
        public AttackTypeEnum AttackType { get; set; } = AttackTypeEnum.None;
        public TriggerConditions TriggerCondition { get; set; } = TriggerConditions.None;
        public int Damage { get; set; }
        public int Penetration { get; set; }
        public CreatureBase ActiveCreature { get; set; }
        public CreatureBase Target { get; set; }
        public StringBuilder SB { get; set; } = new StringBuilder();
    }
}

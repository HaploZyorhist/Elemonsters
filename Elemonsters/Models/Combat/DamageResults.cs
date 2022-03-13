using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat
{
    public class DamageResults
    {
        public AttackTypeEnum AttackType { get; set; }
        public TriggerConditions Trigger { get; set; } = TriggerConditions.None;
        public int Damage { get; set; }
        public CreatureBase Target { get; set; }
        public StringBuilder SB { get; set; } = new StringBuilder();
        public ElementalRequest ElementalRequest { get; set; } = new ElementalRequest();
    }
}

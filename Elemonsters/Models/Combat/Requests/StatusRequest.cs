using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Assets.StatusEffects;

namespace Elemonsters.Models.Combat.Requests
{
    public class StatusRequest
    {
        public CreatureBase Target { get; set; }
        public List<StatusEffect> Statuses { get; set; } = new List<StatusEffect>();
        public StringBuilder SB { get; set; } = new StringBuilder();
    }
}

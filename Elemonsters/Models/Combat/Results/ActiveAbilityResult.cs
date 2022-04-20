using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat.Results
{
    public class ActiveAbilityResult
    {
        public List<DamageRequest> DamageRequests { get; set; } = new List<DamageRequest>();

        public ulong ActivatingCreature { get; set; }

        public TriggerConditionsEnum Triggers { get; set; } = TriggerConditionsEnum.None;

        public StringBuilder Messages { get; set; } = new StringBuilder();
    }
}

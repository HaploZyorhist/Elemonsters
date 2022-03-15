using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat.Requests
{
    /// <summary>
    /// object containing the data on the activation conditions for a passive to trigger
    /// </summary>
    public class CheckTriggersRequest
    {
        public ulong ActivatingCreature { get; set; }
        public TriggerConditions TriggerCondition { get; set; }
        public CreatureBase Target { get; set; }
    }
}

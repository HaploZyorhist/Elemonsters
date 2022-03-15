using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Results;

namespace Elemonsters.Models.Combat.Requests
{
    public class ActiveRequest
    {
        public List<CreatureBase> Targets { get; set; } = new List<CreatureBase>();
        public CreatureBase MyTurn { get; set; } = new CreatureBase();
        public string AbilityName { get; set; } = "";
        public int AbilityLevel { get; set; } = 0;
    }
}

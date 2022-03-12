using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat
{
    public class PassiveRequest
    {
        public BattleContainer Container { get; set; } = new BattleContainer();
        public CreatureBase MyTurn { get; set; } = new CreatureBase();
        public List<CreatureBase> Targets { get; set; } = new List<CreatureBase>();
        public string AbilityName { get; set; } = "";
        public int AbilityLevel { get; set; } = 0;
    }
}

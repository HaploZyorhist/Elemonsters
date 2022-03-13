using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat
{
    public class ActiveResults
    {
        public BattleContainer Container { get; set; }
        public List<DamageResults> DamageResults { get; set; } = new List<DamageResults>();
    }
}

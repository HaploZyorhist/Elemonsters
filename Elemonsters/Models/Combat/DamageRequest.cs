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
    /// object containing data for requesting damage information
    /// </summary>
    public class DamageRequest
    {
        public AttackTypeEnum AttackType { get; set; }
        public int Damage { get; set; }
        public int Penetration { get; set; }
        public int Defense { get; set; }
        public double DamageModifier { get; set; }
    }
}

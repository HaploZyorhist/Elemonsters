using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Enums;

namespace Elemonsters.Assets.Creatures
{
    /// <summary>
    /// object containing element types and values for upgrading
    /// </summary>
    public class CreatureElements
    {
        public PhysicalElement PhysicalElement { get; set; }
        public int PhysicalValue { get; set; }
        public RangedElement RangedElement { get; set; }
        public int RangedValue { get; set; }
    }
}

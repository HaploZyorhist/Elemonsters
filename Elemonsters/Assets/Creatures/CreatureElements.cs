using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures
{
    /// <summary>
    /// object containing element types and values for upgrading
    /// </summary>
    public class CreatureElements
    {
        public PhysicalElement PhysicalElement { get; set; }
        public int PhysicalValue { get; set; }
        public MagicElement MagicElement { get; set; }
        public int RangedValue { get; set; }
    }
}

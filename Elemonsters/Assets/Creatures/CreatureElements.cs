using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures
{
    /// <summary>
    /// object containing element types and values for upgrading
    /// </summary>
    public class CreatureElements
    {
        /// <summary>
        /// The creature's physical element
        /// </summary>
        public PhysicalElement PhysicalElement { get; set; }

        /// <summary>
        /// how much resource has been put into the physical element
        /// </summary>
        public int PhysicalValue { get; set; }

        /// <summary>
        /// the creature's element for ranged attacks
        /// </summary>
        public MagicElement RangedElement { get; set; }

        /// <summary>
        /// how much resource has been put into the ranged element
        /// </summary>
        public int RangedValue { get; set; }
    }
}

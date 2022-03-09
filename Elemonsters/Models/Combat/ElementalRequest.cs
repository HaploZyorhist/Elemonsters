using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat
{
    /// <summary>
    /// object containing data on getting elemental bonuses
    /// </summary>
    public class ElementalRequest
    {
        public AttackTypeEnum AttackType { get; set; }
        public CreatureElements AttackerElements { get; set; }
        public CreatureElements DefenderElements { get; set; }
    }
}

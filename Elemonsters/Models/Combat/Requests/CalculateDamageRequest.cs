using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat.Requests
{
    public class CalculateDamageRequest
    {
        public AttackTypeEnum AttackType { get; set; }
        public int Damage { get; set; }
        public int Penetration { get; set; }
    }
}

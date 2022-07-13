using System.Text;

namespace Elemonsters.Models.DamageFactory.Results
{
    public class DamageFactoryResults
    {
        public ulong Target { get; set; }
        public int TrueDamage { get; set; }
        public int ElementalDamage { get; set; }
        public int PhysicalDamage { get; set; }
        public StringBuilder SB { get; set; } = new StringBuilder();
    }
}

using Elemonsters.Models.DamageFactory.Requests;

namespace Elemonsters.Models.DamageFactory.Requests
{
    public class DamageFactoryRequest
    {
        public List<DamageFactoryData> DamageFactoryData { get; set; } = new List<DamageFactoryData>();
        public int TargetDefense { get; set; }
        public int TargetAura { get; set; }
    }
}

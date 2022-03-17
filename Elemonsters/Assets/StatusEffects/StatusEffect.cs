using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.StatusEffects
{
    public class StatusEffect
    {
        public string Name { get; set; }
        public EffectTypes Type { get; set; }
        public AddStatusEnum Add { get; set; }
        public RemoveStatusEnum Remove { get; set; }
        public int Duration { get; set; }
        public int Stacks { get; set; }
        public int Value { get; set; }
        public bool ToDisplay { get; set; } = true;
    }
}

using Elemonsters.Assets.Creatures;
using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat.Results
{
    /// <summary>
    /// result object containing the options for targeting
    /// </summary>
    public class TargetRulesResult
    {
        public TargetingRulesEnum Rule { get; set; } = TargetingRulesEnum.Standard;
        public StatusEffect Filter { get; set; } = null;
        public int TotalTargets { get; set; } = 1;
        public bool UniqueTargets { get; set; } = true;
    }
}

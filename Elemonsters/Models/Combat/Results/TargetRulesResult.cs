using Elemonsters.Assets.StatusEffects;
using Elemonsters.Models.Enums;

namespace Elemonsters.Models.Combat.Results
{
    /// <summary>
    /// result object containing the options for targeting
    /// </summary>
    public class TargetRulesResult
    {
        public TargetingRulesEnum Rule { get; set; } = TargetingRulesEnum.FrontToBack;
        public BuffDebuff Filter { get; set; } = null;
        public int TotalTargets { get; set; } = 1;
        public bool UniqueTargets { get; set; } = true;
    }
}

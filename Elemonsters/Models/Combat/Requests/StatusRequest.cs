using Elemonsters.Assets.StatusEffects;

namespace Elemonsters.Models.Combat.Requests
{

    /// <summary>
    /// request for applying statuses to creature
    /// </summary>
    public class StatusRequest
    {
        /// <summary>
        /// id of creature that the effects are being applied to
        /// </summary>
        public ulong Target { get; set; }

        /// <summary>
        /// list of effects to be applied to the creature
        /// </summary>
        public List<StatusEffect> Statuses { get; set; } = new List<StatusEffect>();
    }
}

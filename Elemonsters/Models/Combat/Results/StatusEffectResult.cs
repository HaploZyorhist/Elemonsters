using Elemonsters.Models.Combat.Requests;

namespace Elemonsters.Models.Combat.Results
{
    /// <summary>
    /// results for a passive activation
    /// </summary>
    public class StatusEffectResult
    {
        /// <summary>
        /// list of damage requests coming from an active ability
        /// </summary>
        public List<DamageRequest> DamageRequests { get; set; } = new List<DamageRequest>();
    }
}

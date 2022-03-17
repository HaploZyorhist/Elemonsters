using Elemonsters.Models.Combat.Requests;

namespace Elemonsters.Models.Combat.Results
{
    /// <summary>
    /// results for a passive activation
    /// </summary>
    public class PassiveResult
    {
        /// <summary>
        /// list of damage requests coming from an active ability
        /// </summary>
        public List<DamageRequest> DamageRequests { get; set; } = new List<DamageRequest>();

        /// <summary>
        /// list of status requests coming from an active ability
        /// </summary>
        public List<StatusRequest> StatusRequests { get; set; } = new List<StatusRequest>();
    }
}

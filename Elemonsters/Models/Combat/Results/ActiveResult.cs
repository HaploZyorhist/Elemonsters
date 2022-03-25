using System.Text;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.StatusEffects.Results;

namespace Elemonsters.Models.Combat.Results
{
    /// <summary>
    /// Object for returning results of an ability
    /// </summary>
    public class ActiveResult
    {
        /// <summary>
        /// list of damage requests coming from an active ability
        /// </summary>
        public List<DamageRequest> DamageRequests { get; set; } = new List<DamageRequest>();

        /// <summary>
        /// object containing updated statuses of creatures effected by status effects
        /// </summary>
        public AddStatusEffectResult StatusEffectResults { get; set; } = new AddStatusEffectResult();

        /// <summary>
        /// string builder containing information from the activation off the ability
        /// </summary>
        public StringBuilder SB { get; set; } = new StringBuilder();
    }
}

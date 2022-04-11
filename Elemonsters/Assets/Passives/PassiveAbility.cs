using Elemonsters.Models.StatusEffects.Requests;
using Elemonsters.Models.StatusEffects.Results;

namespace Elemonsters.Assets.Passives
{
    /// <summary>
    /// Ability subclass for passive effects
    /// </summary>
    public class PassiveAbility
    {
        /// <summary>
        /// method for adding the status effect to the creature
        /// </summary>
        /// <param name="request">object containing the data about what the status effect is being added to</param>
        /// <returns>object containing the results of the status effect add</returns>
        public virtual async Task<AddStatusEffectResult> AddStatusEffect(AddStatusEffectRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

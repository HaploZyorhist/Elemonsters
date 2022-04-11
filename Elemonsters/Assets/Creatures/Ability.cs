using Elemonsters.Assets.Creatures.PassiveAbilities;
using Elemonsters.Assets.Passives;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures
{
    /// <summary>
    /// parent class for abilities
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// name of the ability
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// indicates if the ability has an active effect
        /// </summary>
        public bool IsActive { get; set; } = false;

        /// <summary>
        /// what slot the ability is in
        /// </summary>
        public AbilitySlotEnum AbilitySlot { get; set; }

        /// <summary>
        /// the cost to activate the ability
        /// </summary>
        public int Cost { get; set; } = 0;

        /// <summary>
        /// the creature's passive ability
        /// </summary>
        public List<PassiveAbility> PassiveAbilities { get; set; } = new List<PassiveAbility>();

        /// <summary>
        /// method for activating ability
        /// </summary>
        /// <param name="request">request object containing data for activation</param>
        /// <returns>list of results to be processed and applied to creatures</returns>
        public virtual async Task<ActiveResult> Activation(ActiveRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// method for getting targets for ability
        /// </summary>
        /// <param name="request">request object with data for target selection</param>
        /// <returns>object containing options on what targets are available</returns>
        public virtual async Task<TargetRulesResult> GetTargetOptions()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.PassiveAbilities
{
    /// <summary>
    /// Ability subclass for passive effects
    /// </summary>
    public class PassiveAbility
    {
        /// <summary>
        /// the condition that can trigger this ability
        /// </summary>
        public TriggerConditions TriggerConditions { get; set; } = TriggerConditions.None;

        /// <summary>
        /// the creatures who are able to trigger this ability
        /// </summary>
        public List<ulong> AllowedActivators { get; set; } = new List<ulong>();

        /// <summary>
        /// the method that triggers when the trigger conditions are met
        /// </summary>
        /// <param name="request">request object used for activating the passive</param>
        /// <returns>result object containing the details of what the passive did</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<PassiveResult> Passive(PassiveRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

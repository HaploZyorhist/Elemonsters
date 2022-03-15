using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Combat.Results;

namespace Elemonsters.Assets.Creatures.ActiveAbilities
{
    /// <summary>
    /// base class for active abilities
    /// </summary>
    public class ActiveAbility
    {
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
        public virtual async Task<GetTargetsResult> GetTargetOptions(GetTargetsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

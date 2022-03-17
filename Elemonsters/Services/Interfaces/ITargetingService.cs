using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Requests;

namespace Elemonsters.Services.Interfaces
{
    public interface ITargetingService
    {
        /// <summary>
        /// Method for getting target options based on the rules of the ability
        /// </summary>
        /// <param name="request">details on the rules of the target options</param>
        /// <returns>list of creatures that the player has chosen to hit</returns>
        Task<List<CreatureBase>> GetTargets(GetTargetsRequest request);
    }
}

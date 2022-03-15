using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat.Requests;

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
        /// list of status requests coming from an active ability
        /// </summary>
        public List<StatusRequest> StatusRequests { get; set; } = new List<StatusRequest>();
    }
}

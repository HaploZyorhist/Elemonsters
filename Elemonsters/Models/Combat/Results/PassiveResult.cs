using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat.Requests;

namespace Elemonsters.Models.Combat.Results
{
    public class PassiveResult
    {
        public List<DamageRequest> DamageRequests { get; set; } = new List<DamageRequest>();

        /// <summary>
        /// list of status requests coming from an active ability
        /// </summary>
        public List<StatusRequest> StatusRequests { get; set; } = new List<StatusRequest>();
    }
}

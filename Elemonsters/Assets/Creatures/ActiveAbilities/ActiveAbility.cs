using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;

namespace Elemonsters.Assets.Creatures.ActiveAbilities
{
    public class ActiveAbility
    {
        public virtual async Task<ActiveResults> Activation(ActiveRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

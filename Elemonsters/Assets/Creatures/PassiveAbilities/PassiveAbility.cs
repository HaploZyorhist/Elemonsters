using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.PassiveAbilities
{
    /// <summary>
    /// Ability subclass for passive effects
    /// </summary>
    public class PassiveAbility
    {
        public TriggerConditions TriggerConditions { get; set; } = TriggerConditions.None;

        public virtual async Task<PassiveResults> Passive(PassiveRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

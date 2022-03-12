using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat;

namespace Elemonsters.Assets.Creatures.PassiveAbilities
{
    /// <summary>
    /// Ability subclass for passive effects
    /// </summary>
    public class PassiveAbility
    {
        public string ActivationCondition = "this";

        public virtual async Task<List<CombatResults>> Passive(BattleContainer battle, CreatureBase myTurn)
        {
            throw new NotImplementedException();
        }
    }
}

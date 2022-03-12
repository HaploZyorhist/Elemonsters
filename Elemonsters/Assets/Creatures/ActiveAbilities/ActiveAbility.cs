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
        public virtual async Task<List<CombatResults>> Activation(BattleContainer battleContainer, CreatureBase myTurn)
        {
            throw new NotImplementedException();
        }
    }
}

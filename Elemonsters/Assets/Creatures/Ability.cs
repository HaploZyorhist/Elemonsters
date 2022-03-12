using Elemonsters.Models.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures.ActiveAbilities;
using Elemonsters.Assets.Creatures.PassiveAbilities;
using Elemonsters.Factories;

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

        public int AbilityLevel { get; set; } = 0;

        public PassiveAbility PassiveAbility { get; set; }

        public ActiveAbility ActiveAbility { get; set; }
    }
}

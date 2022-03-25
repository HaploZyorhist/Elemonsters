using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat;

namespace Elemonsters.Models.StatusEffects.Requests
{
    /// <summary>
    /// object for adding status effects to a creature
    /// </summary>
    public class AddStatusEffectRequest
    {
        /// <summary>
        /// the ability who's passive is being added
        /// </summary>
        public Ability Ability { get; set; } = new Ability();

        /// <summary>
        /// object containing all the data for the battle
        /// </summary>
        public BattleContainer Container { get; set; } = new BattleContainer();

        /// <summary>
        /// target of the status effect
        /// </summary>
        public ulong Target { get; set; }
    }
}

using Elemonsters.Assets.Creatures.ActiveAbilities;
using Elemonsters.Assets.Creatures.PassiveAbilities;
using Elemonsters.Models.Enums;

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

        /// <summary>
        /// the ability level
        /// </summary>
        public int AbilityLevel { get; set; } = 0;

        /// <summary>
        /// what type the ability is of
        /// </summary>
        public AbilityTypes AbilityType { get; set; }

        /// <summary>
        /// the creature's passive ability
        /// </summary>
        public PassiveAbility PassiveAbility { get; set; }

        /// <summary>
        /// the creature's active ability
        /// </summary>
        public ActiveAbility ActiveAbility { get; set; }
    }
}

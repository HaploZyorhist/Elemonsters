using System.Text;
using Elemonsters.Models.Combat;

namespace Elemonsters.Models.StatusEffects.Results
{
    /// <summary>
    /// result object containing data from adding a new status effect
    /// </summary>
    public class AddStatusEffectResult
    {
        /// <summary>
        /// battle container containing data on the battle 
        /// </summary>
        public BattleContainer Container { get; set; } = new BattleContainer();

        /// <summary>
        /// string builder containing messages about the results of the method
        /// </summary>
        public StringBuilder SB { get; set; } = new StringBuilder();
    }
}

using Discord;
using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat
{
    public class BattleContainer
    {
        public List<IUser> Players { get; set; } = new List<IUser>();

        public List<CreatureBase> Creatures { get; set; } = new List<CreatureBase>();

        public int Instance { get; set; } = 0;
    }
}

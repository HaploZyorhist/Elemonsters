using Discord;
using Discord.Commands;
using Elemonsters.Assets.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Models.Combat
{
    public class BattleContainer
    {
        public List<IUser> Players { get; set; }

        public List<CreatureBase> Creatures { get; set; }

        public int Instance { get; set; }

        public ICommandContext Context { get; set; }
    }
}

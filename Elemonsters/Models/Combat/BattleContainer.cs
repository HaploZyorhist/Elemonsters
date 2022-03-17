using Discord;
using Discord.Commands;
using Elemonsters.Assets.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Factories;

namespace Elemonsters.Models.Combat
{
    public class BattleContainer
    {
        public List<IUser> Players { get; set; } = new List<IUser>();

        public List<CreatureBase> Creatures { get; set; } = new List<CreatureBase>();

        public int Instance { get; set; } = 0;
    }
}

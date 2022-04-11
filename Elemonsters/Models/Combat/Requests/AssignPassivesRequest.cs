using Discord.Commands;
using Elemonsters.Assets.Creatures;

namespace Elemonsters.Models.Combat.Requests
{
    public class AssignPassivesRequest
    {
        public List<CreatureBase> Creatures { get; set; }=new List<CreatureBase>();
        public ICommandContext Context { get; set; }
    }
}

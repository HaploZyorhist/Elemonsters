using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Services
{
    /// <summary>
    /// class for handling party related services
    /// </summary>
    public class PartyService : IPartyService
    {
        public StatFactory _statFactory = new StatFactory();
        /// <inheritdoc />
        public CreatureParent SpawnCreature(CreatureParent creature)
        {
            try
            {
                //TODO Get data from database for creature upgrade status

                var c = creature;

                c.Level = 1;
                c.Rank = 1;
                c.Stats = _statFactory.GenerateStats(c);

                return c;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

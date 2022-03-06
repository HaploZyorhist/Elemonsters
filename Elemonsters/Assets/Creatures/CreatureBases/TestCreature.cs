﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Assets.Creatures.CreatureBases
{
    /// <summary>
    /// this creature is used only for test purposes
    /// </summary>
    public class TestCreature : CreatureParent
    {
        public TestCreature()
        {
            Name = "Testy";
            User = 0;
            Stats = new CreatureStats
            {
                Health = 1000,
                MaxHealth = 1000,
                Energy = 100,
                MaxEnergy = 100,
                Strength = 100,
                Defense = 100,
                Lethality = 10,
                Spirit = 100,
                Aura = 100,
                Sorcery = 10,
                CritChance = 50,
                CritModifier = 150,
                Dodge = 10,
                Tenacity = 10,
                Regeneration = 10,
                Speed = 10,
                Vamp = 10,
                Drain = 10,
            };
            Level = 0;
            Rank = 0;
        }

        public override Task Ability1()
        {
            throw new NotImplementedException();
        }

        public override Task Ability2()
        {
            throw new NotImplementedException();
        }

        public override Task Ability3()
        {
            throw new NotImplementedException();
        }

        public override Task Passive()
        {
            throw new NotImplementedException();
        }
    }
}
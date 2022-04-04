using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Assets.Creatures.AbilitiesList;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.CreaturesList
{
    public class TestyBoi : CreatureBase
    {
        public TestyBoi()
        {
            Name = "Testy";
            IsLeader = false;
            CreatureID = 0;
            User = 0;
            Level = 1;
            Rank = RankEnum.Bronze;
            ActionPoints = 0;
            Position = PositionEnum.Melee;
            Stats = new CreatureStats
            {
                Strength = 100,
                Defense = 100,
                Lethality = 10,
                Spirit = 100,
                Aura = 100,
                Sorcery = 10,
                CritChance = 100,
                CritModifier = 150,
                Dodge = 100,
                Tenacity = 100,
                MaxHealth = 1000,
                Health = 1000,
                MaxEnergy = 100,
                Energy = 100,
                Regeneration = 100,
                Speed = 10,
            };
            Elements = new CreatureElements
            {
                PhysicalElement = PhysicalElementEnum.Fire,
                PhysicalValue = 100,
                RangedElement = MagicElementEnum.Wind,
                RangedValue = 100,
            };

            Abilities.Add
            (new BasicAttack
            {
                AbilityLevel = 1,
                AbilitySlot = AbilitySlotEnum.BasicAttack,
                IsActive = true,
                Name = "Basic Attack",
                IsLearned = true,
            });

            Abilities.Add
            (new Blight
            {
                AbilityLevel = Level,
                AbilitySlot = AbilitySlotEnum.FirstAbility,
                IsActive = false,
                Name = "Blight",
                IsLearned = true,
            });

            Abilities.Add
            (new GenerateShield
            {
                AbilityLevel = Level,
                AbilitySlot = AbilitySlotEnum.SecondAbility,
                IsActive = true,
                Name = "Total Defense",
                IsLearned = true,
                Cost = 25,
            });
        }
    }
}

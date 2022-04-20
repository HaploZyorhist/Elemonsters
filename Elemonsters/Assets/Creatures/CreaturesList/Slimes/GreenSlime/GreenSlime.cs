using Elemonsters.Assets.Abilities;
using Elemonsters.Assets.Creatures.AbilitiesList;
using Elemonsters.Assets.Creatures.CreaturesList.Slimes.AbilitiesList;
using Elemonsters.Models.Enums;

namespace Elemonsters.Assets.Creatures.CreaturesList.Slimes.GreenSlime
{
    public class GreenSlime : CreatureBase
    {
        public GreenSlime()
        {
            Name = "Green Slime";
            Species = "Slime";
            IsLeader = false;
            CreatureID = 0;
            User = 0;
            Level = 1;
            Rank = RankEnum.Bronze;
            ActionPoints = 0;
            Position = PositionEnum.Melee;
            Stats = new CreatureStats
            {
                Strength = 50,
                Defense = 200,
                Lethality = 10,
                Spirit = 200,
                Aura = 150,
                Sorcery = 10,
                CritChance = 10,
                CritModifier = 150,
                Dodge = 20,
                Tenacity = 100,
                MaxHealth = 1500,
                Health = 1000,
                MaxEnergy = 100,
                Energy = 100,
                Regeneration = 10,
                Speed = 10,
            };

            Abilities.Add
            (
                new AcidHeal
                {
                    AbilitySlot = AbilitySlotEnum.Passive,
                }
            );

            Abilities.Add
            (new Acid()
            {
                AbilitySlot = AbilitySlotEnum.BasicAttack,
            });

            Abilities.Add
            (new Blight
            {
                AbilitySlot = AbilitySlotEnum.FirstAbility,
            });

            Abilities.Add
            (new GenerateShield
            {
                AbilitySlot = AbilitySlotEnum.SecondAbility,
                Cost = 25,
            });
        }
    }
}

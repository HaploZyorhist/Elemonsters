using Elemonsters.Assets.Creatures;
using Elemonsters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Factories
{
    public class DamageFactory
    {
        public async Task<int> CalculateDamage(AttackTypeEnum attackType, CreatureBase attacker, CreatureBase defender)
        {
            try
            {
                switch (attackType)
                {
                    case AttackTypeEnum.True:
                        return attacker.Stats.Strength;

                    case AttackTypeEnum.Physical:
                        return await CalculatePhysicalDamage(attacker, defender);

                    case AttackTypeEnum.Magic:
                        return await CalculateMagicDamage(attacker, defender);

                    default:
                        return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private async Task<int> CalculatePhysicalDamage(CreatureBase attacker, CreatureBase defender)
        {
            try
            {
                double multiplier;
                if (defender.Stats.Defense >= 0)
                {
                    multiplier = 100 / (100 + (double)defender.Stats.Defense);
                }
                else
                {
                    multiplier = 2 - (100 / (100 - (double)defender.Stats.Defense));
                }

                var damage = multiplier * attacker.Stats.Strength;

                int damageRounded = (int)damage;

                return damageRounded;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private async Task<int> CalculateMagicDamage(CreatureBase attacker, CreatureBase defender)
        {
            try
            {
                double multiplier;
                if (defender.Stats.Aura >= 0)
                {
                    multiplier = 100 / (100 + (double)defender.Stats.Aura);
                }
                else
                {
                    multiplier = 2 - (100 / (100 - (double)defender.Stats.Aura));
                }

                var damage = multiplier * attacker.Stats.Spirit;

                int damageRounded = (int)damage;

                return damageRounded;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}

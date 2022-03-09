using Elemonsters.Assets.Creatures;
using Elemonsters.Models;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elemonsters.Factories
{
    public class DamageFactory
    {
        public async Task<int> CalculateDamage(DamageRequest request)
        {
            try
            {
                if (request.AttackType == AttackTypeEnum.True)
                {
                    return request.Damage;
                }

                double multiplier;

                if (request.Defense >= 0)
                {
                    multiplier = 100 / (100 + (double)request.Defense);
                }
                else
                {
                    multiplier = 2 - (100 / (100 - (double)request.Defense));
                }

                var damage = multiplier * request.Damage * request.DamageModifier;

                int damageRounded = (int)damage;

                return damageRounded;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// calculator for getting elemental bonuses
        /// </summary>
        /// <param name="request">request object containing data on the attack</param>
        /// <returns>double for the modifier of to be given to the damage</returns>
        public async Task<double> CheckElementalBonus(ElementalRequest request)
        {
            try
            {
                switch (request.AttackType)
                {
                    case AttackTypeEnum.True:
                        return 1;

                    case AttackTypeEnum.Physical:
                        if (request.AttackerElements.PhysicalElement == request.DefenderElements.PhysicalElement)
                        {
                            return 1;
                        }
                        else if ((request.AttackerElements.PhysicalElement == PhysicalElement.Fire &&
                                  request.DefenderElements.PhysicalElement == PhysicalElement.Wood) ||
                                 (request.AttackerElements.PhysicalElement == PhysicalElement.Water &&
                                  request.DefenderElements.PhysicalElement == PhysicalElement.Fire) ||
                                 (request.AttackerElements.PhysicalElement == PhysicalElement.Wood &&
                                  request.DefenderElements.PhysicalElement == PhysicalElement.Water))
                        {
                            return 1.25;
                        }
                        else
                        {
                            return .8;
                        };

                    case AttackTypeEnum.Magic:
                        if (request.AttackerElements.MagicElement == request.DefenderElements.MagicElement)
                        {
                            return 1;
                        }
                        else if ((request.AttackerElements.MagicElement == MagicElement.Electric &&
                                  request.DefenderElements.MagicElement == MagicElement.Wind) ||
                                 (request.AttackerElements.MagicElement == MagicElement.Earth &&
                                  request.DefenderElements.MagicElement == MagicElement.Electric) ||
                                 (request.AttackerElements.MagicElement == MagicElement.Wind &&
                                  request.DefenderElements.MagicElement == MagicElement.Earth))
                        {
                            return 1.25;
                        }
                        else
                        {
                            return .8;
                        };

                    default:
                        return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}

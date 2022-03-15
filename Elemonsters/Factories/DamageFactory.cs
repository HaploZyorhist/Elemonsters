using Elemonsters.Assets.Creatures;
using Elemonsters.Models;
using Elemonsters.Models.Combat;
using Elemonsters.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elemonsters.Models.Combat.Requests;

namespace Elemonsters.Factories
{
    public class DamageFactory
    {
        public async Task<int> CalculateDamage(DamageRequest request)
        {
            try
            {
                // true damage doesn't care about defenses
                if (request.AttackType == AttackTypeEnum.True)
                {
                    return request.Damage;
                }

                // setup a multiplier for damage modification from defenses
                double multiplier;

                // defense value to be used for calculating damage
                int defense;

                // get defense values for applicable attack types
                if (request.AttackType == AttackTypeEnum.Physical)
                {
                    defense = request.Target.Stats.Defense;
                }
                else
                {
                    defense = request.Target.Stats.Aura;
                }

                // if your defense gets super low we need to make sure your damage amplifies properly
                if (defense >= 0)
                {
                    multiplier = 100 / (100 + (double)defense - request.Penetration);
                }
                else
                {
                    multiplier = 2 - (100 / (100 - (double)defense + request.Penetration));
                }

                // damage delt after resistances
                var damage = multiplier * request.Damage;

                // turning damage into an int
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
        private async Task<double> CheckElementalBonus(CreatureBase myTurn, CreatureBase target, AttackTypeEnum type)
        {
            try
            {
                switch (type)
                {
                    case AttackTypeEnum.True:
                        return 1;

                    case AttackTypeEnum.Physical:
                        if (myTurn.Elements.PhysicalElement == target.Elements.PhysicalElement)
                        {
                            return 1;
                        }
                        else if ((myTurn.Elements.PhysicalElement == PhysicalElement.Fire &&
                                  target.Elements.PhysicalElement == PhysicalElement.Wood) ||
                                 (myTurn.Elements.PhysicalElement == PhysicalElement.Water &&
                                  target.Elements.PhysicalElement == PhysicalElement.Fire) ||
                                 (myTurn.Elements.PhysicalElement == PhysicalElement.Wood &&
                                  target.Elements.PhysicalElement == PhysicalElement.Water))
                        {
                            return 1.25;
                        }
                        else
                        {
                            return .8;
                        };

                    case AttackTypeEnum.Magic:
                        if (myTurn.Elements.MagicElement == target.Elements.MagicElement)
                        {
                            return 1;
                        }
                        else if ((myTurn.Elements.MagicElement == MagicElement.Electric &&
                                  target.Elements.MagicElement == MagicElement.Wind) ||
                                 (myTurn.Elements.MagicElement == MagicElement.Earth &&
                                  target.Elements.MagicElement == MagicElement.Electric) ||
                                 (myTurn.Elements.MagicElement == MagicElement.Wind &&
                                  target.Elements.MagicElement == MagicElement.Earth))
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

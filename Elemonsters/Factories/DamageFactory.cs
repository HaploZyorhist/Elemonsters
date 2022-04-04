﻿using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Enums;
using Elemonsters.Models.Factory.Requests;

namespace Elemonsters.Factories
{
    /// <summary>
    /// factory for handling damage information
    /// </summary>
    public class DamageFactory
    {
        /// <summary>
        /// method for getting damage from an attack
        /// </summary>
        /// <param name="request">data on what damage to be processed</param>
        /// <returns>the amount of damage that is dealt after reductions</returns>
        public async Task<int> CalculateDamage(DamageFactoryRequest request)
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

                // remove penetrated amount from defense
                defense -= request.Penetration;

                // if your defense gets super low we need to make sure your damage amplifies properly
                if (defense >= 0)
                {
                    multiplier = 100 / (100 + (double)defense);
                }
                else
                {
                    multiplier = 2 - 100 / (100 - (double)defense);
                }

                var elementalModifier = await CheckElementalBonus(request.Attacker, request.Target, request.AttackType);

                // damage delt after resistances
                var damage = multiplier * elementalModifier * request.Damage;

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
        private async Task<double> CheckElementalBonus(CreatureBase attacker, CreatureBase target, AttackTypeEnum type)
        {
            try
            {
                switch (type)
                {
                    case AttackTypeEnum.True:
                        return 1;

                    case AttackTypeEnum.Physical:
                        if (attacker.Elements.PhysicalElement == target.Elements.PhysicalElement)
                        {
                            return 1;
                        }
                        else if ((attacker.Elements.PhysicalElement == PhysicalElementEnum.Fire &&
                                  target.Elements.PhysicalElement == PhysicalElementEnum.Wood) ||
                                 (attacker.Elements.PhysicalElement == PhysicalElementEnum.Water &&
                                  target.Elements.PhysicalElement == PhysicalElementEnum.Fire) ||
                                 (attacker.Elements.PhysicalElement == PhysicalElementEnum.Wood &&
                                  target.Elements.PhysicalElement == PhysicalElementEnum.Water))
                        {
                            return 1.25;
                        }
                        else
                        {
                            return .8;
                        };

                    case AttackTypeEnum.Magic:
                        if (attacker.Elements.RangedElement == target.Elements.RangedElement)
                        {
                            return 1;
                        }
                        else if ((attacker.Elements.RangedElement == MagicElementEnum.Electric &&
                                  target.Elements.RangedElement == MagicElementEnum.Wind) ||
                                 (attacker.Elements.RangedElement == MagicElementEnum.Earth &&
                                  target.Elements.RangedElement == MagicElementEnum.Electric) ||
                                 (attacker.Elements.RangedElement == MagicElementEnum.Wind &&
                                  target.Elements.RangedElement == MagicElementEnum.Earth))
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

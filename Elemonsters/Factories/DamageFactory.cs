using Elemonsters.Models.DamageFactory.Requests;
using Elemonsters.Models.DamageFactory.Results;
using Elemonsters.Models.Enums;

namespace Elemonsters.Factories
{
    public class DamageFactory
    {
        /// <summary>
        /// method for processing all damage requests
        /// </summary>
        /// <param name="request">request object dictating the damage that will be dealt to the target</param>
        public async Task<DamageFactoryResults> ProcessDamageRequests(DamageFactoryRequest request)
        {
            var result = new DamageFactoryResults();

            try
            {
                var physicalRequests = request.DamageFactoryData
                    .Where(x => x.AttackType == AttackTypeEnum.Physical)
                    .ToList();

                var physicalCalculationRequests = physicalRequests.Select(x => new DamageCalculationRequest
                {
                    Damage = x.Damage,
                    Defense = request.TargetDefense,
                    Penetration = x.Penetration
                })
                    .ToList();

                var totalPhysicalDamage = physicalCalculationRequests
                        .Sum(x => CalculateDamage(x)
                            .GetAwaiter()
                            .GetResult());

                result.PhysicalDamage = totalPhysicalDamage;

                var elementalRequests = request.DamageFactoryData
                    .Where(x => x.AttackType == AttackTypeEnum.Elemental)
                    .ToList();

                var elementalCalculationRequests = elementalRequests.Select(x => new DamageCalculationRequest
                    {
                        Damage = x.Damage,
                        Defense = request.TargetAura,
                        Penetration = x.Penetration
                    })
                    .ToList();

                var totalElementalDamage = elementalCalculationRequests
                    .Sum(x => CalculateDamage(x)
                        .GetAwaiter()
                        .GetResult());

                result.ElementalDamage = totalElementalDamage;

                var trueDamageRequests = request.DamageFactoryData
                    .Where(x => x.AttackType == AttackTypeEnum.True)
                    .ToList();

                var totalTrueDamage = trueDamageRequests.Sum(x => x.Damage);

                result.TrueDamage = totalTrueDamage;

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// method for getting damage from an attack
        /// </summary>
        /// <param name="request">data on what damage to be processed</param>
        /// <returns>the amount of damage that is dealt after reductions</returns>
        private async Task<int> CalculateDamage(DamageCalculationRequest request)
        {
            try
            {
                // setup a multiplier for damage modification from defenses
                double multiplier;

                // defense value to be used for calculating damage
                int defense = request.Defense;

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
    }
}

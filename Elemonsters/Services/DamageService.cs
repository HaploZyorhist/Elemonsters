using System.Text;
using Elemonsters.Assets.Creatures;
using Elemonsters.Factories;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Damage.Results;
using Elemonsters.Models.Enums;
using Elemonsters.Models.Factory.Requests;
using Elemonsters.Services.Interfaces;

namespace Elemonsters.Services
{
    /// <summary>
    /// service for handling damage requests
    /// </summary>
    public class DamageService : IDamageService
    {
        #region Fields

        private readonly DamageFactory _damageFactory;

        #endregion

        #region CTOR

        public DamageService(DamageFactory damageFactory)
        {
            _damageFactory = damageFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// method for dealing damage to targets
        /// </summary>
        /// <param name="damageRequests">request object dictating how to deal damage to target</param>
        public async Task<DamageResult> HandleDamage(List<DamageRequest> damageRequests, List<CreatureBase> creatures)
        {
            var messages = new StringBuilder();

            try
            {
                var targetList = damageRequests
                    .Select(x => x.Target)
                    .Distinct()
                    .ToList();

                foreach (var target in targetList)
                {
                    var me = creatures
                        .Where(x => x.CreatureID == target)
                        .FirstOrDefault();

                    var requests = damageRequests
                        .Where(x => x.Target == target)
                        .ToList();
                    int totalDamage = 0;

                    var physicalRequests = requests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.Physical)
                        .ToList();

                    var physicalDamageRequests = physicalRequests
                        .Select(x => new DamageFactoryRequest()
                        {
                            Target = me,
                            AttackType = AttackTypeEnum.Physical,
                            Damage = x.Damage,
                            Penetration = x.Penetration,
                            Attacker = creatures
                                .Where(y => y.CreatureID == x.ActiveCreature)
                                .FirstOrDefault(),
                        })
                        .ToList();

                    int physicalDamage = physicalDamageRequests
                        .Sum(x => _damageFactory
                            .CalculateDamage(x)
                            .GetAwaiter()
                            .GetResult());

                    var elementalRequests = requests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.Elemental)
                        .ToList();

                    var elementalDamageRequests = elementalRequests
                        .Select(x => new DamageFactoryRequest()
                        {
                            Target = me,
                            AttackType = AttackTypeEnum.Elemental,
                            Damage = x.Damage,
                            Penetration = x.Penetration,
                            Attacker = creatures
                                .Where(y => y.CreatureID == x.ActiveCreature)
                                .FirstOrDefault(),
                        })
                        .ToList();

                    int elementalDamage = elementalDamageRequests
                        .Sum(x => _damageFactory
                            .CalculateDamage(x)
                            .GetAwaiter()
                            .GetResult());

                    var trueRequests = requests
                        .Where(x => x.Target == target &&
                                    x.AttackType == AttackTypeEnum.True)
                        .ToList();

                    var trueDamageRequests = trueRequests
                        .Select(x => new DamageFactoryRequest
                        {
                            Target = me,
                            AttackType = AttackTypeEnum.True,
                            Damage = x.Damage,
                            Penetration = x.Penetration
                        });

                    var trueDamage = trueDamageRequests
                        .Sum(x => _damageFactory
                            .CalculateDamage(x)
                            .GetAwaiter()
                            .GetResult());

                    totalDamage = physicalDamage + elementalDamage + trueDamage;

                    var elementalShields = me.Statuses
                        .Where(x =>
                            x.EffectType == EffectTypesEnum.ElementalShield)
                        .OrderBy(x => x.Duration)
                        .ToList();

                    if (elementalShields != null && elementalShields.Count > 0)
                    {
                        var totalElementalShield = elementalShields
                            .Select(x => x.Value)
                            .Sum();

                        messages
                            .AppendLine($"<@{me.User}>'s {me.Name} currently has {totalElementalShield} Elemental Shield");
                    }

                    var physicalShields = me.Statuses
                        .Where(x =>
                            x.EffectType == EffectTypesEnum.PhysicalShield)
                        .OrderBy(x => x.Duration)
                        .ToList();

                    if (physicalShields != null && physicalShields.Count > 0)
                    {
                        var totalPhysicalShield = physicalShields
                            .Select(x => x.Value)
                            .Sum();

                        messages
                            .AppendLine($"<@{me.User}>'s {me.Name} currently has {totalPhysicalShield} Physical Shield");
                    }

                    var generalShields = me.Statuses
                        .Where(x =>
                            x.EffectType == EffectTypesEnum.GeneralShield)
                        .OrderBy(x => x.Duration)
                        .ToList();

                    if (generalShields != null && generalShields.Count > 0)
                    {
                        var totalGeneralShield = generalShields
                            .Select(x => x.Value)
                            .Sum();

                        messages
                            .AppendLine($"<@{me.User}>'s {me.Name} currently has {totalGeneralShield} General Shield");
                    }

                    int remainingDamage = 0;

                    int remainingElementalDamage = elementalDamage;
                    int remainingPhysicalDamage = physicalDamage;

                    while (elementalShields != null &&
                           elementalShields.Count > 0 &&
                           remainingElementalDamage > 0)
                    {
                        var currentShield = elementalShields.First();

                        if (currentShield.Value > remainingElementalDamage)
                        {
                            currentShield.Value -= remainingElementalDamage;
                            remainingElementalDamage = 0;
                        }
                        else
                        {
                            remainingElementalDamage -= currentShield.Value;
                            elementalShields.Remove(currentShield);
                        }
                    }

                    remainingDamage = remainingElementalDamage;

                    while (physicalShields != null &&
                           physicalShields.Count > 0 &&
                           remainingPhysicalDamage > 0)
                    {
                        var currentShield = physicalShields.First();

                        if (currentShield.Value > remainingPhysicalDamage)
                        {
                            currentShield.Value -= remainingPhysicalDamage;
                            remainingPhysicalDamage = 0;
                        }
                        else
                        {
                            remainingPhysicalDamage -= currentShield.Value;
                            physicalShields.Remove(currentShield);
                        }
                    }

                    remainingDamage += remainingPhysicalDamage + trueDamage;

                    while (generalShields != null &&
                           generalShields.Count > 0 &&
                           remainingDamage > 0)
                    {
                        var currentShield = generalShields.First();

                        if (currentShield.Value > remainingDamage)
                        {
                            currentShield.Value -= remainingDamage;
                            remainingDamage = 0;
                        }
                        else
                        {
                            remainingDamage -= currentShield.Value;
                            generalShields.Remove(currentShield);
                        }
                    }

                    if (remainingDamage > 0)
                    {
                        me.Stats.Health -= remainingDamage;
                    }

                    if (me.Stats.Health < 0)
                    {
                        me.Stats.Health = 0;
                    }

                    if (elementalDamage != remainingElementalDamage)
                    {
                        messages.AppendLine(
                            $"<@{me.User}>'s {me.Name}'s Elemental Shield has blocked {elementalDamage - remainingElementalDamage} damage");
                    }

                    if (physicalDamage != remainingPhysicalDamage)
                    {
                        messages.AppendLine(
                            $"<@{me.User}>'s {me.Name}'s Physical Shield has blocked {physicalDamage - remainingPhysicalDamage} damage");
                    }

                    if (totalDamage != remainingDamage)
                    {
                        messages.AppendLine(
                            $"<@{me.User}>'s {me.Name}'s Elemental Shield has blocked {totalDamage - remainingDamage} damage");
                    }
                    
                    messages.
                        AppendLine($"<@{me.User}>'s {me.Name} took {totalDamage} total damage, {totalDamage - remainingDamage} was blocked by shields.  " +
                                   $"{remainingDamage} was dealt to their health");

                    messages
                        .AppendLine($"{remainingElementalDamage} was Elemental Damage, {remainingPhysicalDamage} was Physical Damage, {trueDamage} was True Damage");

                    if (me.Stats.Health == 0)
                    {
                        messages.AppendLine($"<@{me.User}>'s {me.Name} has died");
                    }

                    messages.AppendLine(
                        $"<@{me.User}>'s {me.Name} has {me.Stats.Health} remaining health");

                    me.Statuses.RemoveAll(x => (x.EffectType == EffectTypesEnum.ElementalShield ||
                                                x.EffectType == EffectTypesEnum.PhysicalShield ||
                                                x.EffectType == EffectTypesEnum.GeneralShield) && x.Value <= 0);
                }

                var result = new DamageResult
                {
                    Messages = messages
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

    }
}

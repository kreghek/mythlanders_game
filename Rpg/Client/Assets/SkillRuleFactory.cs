using System.Collections.Generic;

using Client.Core.Skills;

using Rpg.Client.Assets.SkillEffects;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets
{
    internal static class SkillRuleFactory
    {
        public static EffectRule CreateDamage(SkillSid sid)
        {
            return CreateDamage(sid, SkillDirection.Target, 1f);
        }

        public static EffectRule CreateDamage(SkillSid sid, ITargetSelector direction)
        {
            return CreateDamage(sid, direction, 1f);
        }

        public static EffectRule CreateDamage(SkillSid sid, ITargetSelector direction, float multiplier)
        {
            return CreateDamage(sid, direction, multiplier, scatter: 0.1f);
        }

        public static EffectRule CreateDamage(SkillSid sid, ITargetSelector direction, float multiplier, float scatter)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(actor =>
                {
                    var equipmentMultiplierBonus = actor.Unit.GetEquipmentDamageMultiplierBonus(sid);

                    var res = new DamageEffect(actor)
                    {
                        DamageMultiplier = multiplier * (1 + equipmentMultiplierBonus),
                        Scatter = scatter
                    };

                    return res;
                })
            };
        }

        public static EffectRule CreatePeriodicDamage(SkillSid sid, int duration, ITargetSelector direction)
        {
            return CreatePeriodicDamage(sid, power: 1f, duration, direction);
        }

        public static EffectRule CreatePeriodicDamage(SkillSid sid, float power, int duration,
            ITargetSelector direction)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new DurationEffectLifetime(compensationDuration);
                    var effect = new PeriodicDamageEffect(u, effectLifetime)
                    {
                        PowerMultiplier = power,
                        Visualization = EffectVisualizations.Damage
                    };

                    return effect;
                })
            };
        }

        public static EffectRule CreatePeriodicHealing(SkillSid sid, float power, int duration,
            ITargetSelector direction)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplierBonus = u.Unit.GetEquipmentHealMultiplierBonus(sid);

                    var effectLifetime = new DurationEffectLifetime(compensationDuration);

                    var effect = new PeriodicHealEffect(u, effectLifetime)
                    {
                        PowerMultiplier = power * (1 + equipmentMultiplierBonus),
                        Visualization = EffectVisualizations.Healing
                    };

                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerDown(SkillSid sid, ITargetSelector direction, int duration)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new DurationEffectLifetime(compensationDuration);
                    var effect = new ModifyDamageEffect(u, effectLifetime, bonus: -u.Unit.Support)
                    {
                        Visualization = EffectVisualizations.PowerDown
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerDown(SkillSid sid, ITargetSelector direction, int duration, int penalty)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new DurationEffectLifetime(compensationDuration);
                    var effect = new ModifyDamageEffect(u, effectLifetime, bonus: -penalty)
                    {
                        Visualization = EffectVisualizations.PowerDown
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerUp(int equipmentLevel, ITargetSelector direction,
            float equipmentMultiplier = 0.5f)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new UnitBoundEffectLifetime(u);
                    var effectMultiplier = (equipmentLevel + 1) * equipmentMultiplier;
                    var effect = new ModifyDamagePercentEffect(u, effectLifetime, effectMultiplier)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerUp(SkillSid sid, ITargetSelector direction)
        {
            return CreatePowerUp(sid, direction, duration: 1);
        }

        public static EffectRule CreatePowerUp(SkillSid sid, ITargetSelector direction, int duration)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var durationEffectLifetime = new DurationEffectLifetime(compensationDuration);
                    var effect = new ModifyDamageEffect(u, durationEffectLifetime, bonus: u.Unit.Support)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerUpAura(ITargetSelector direction, float multiplier = 0.5f)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new UnitBoundEffectLifetime(u);
                    var effect = new ModifyDamagePercentEffect(u, effectLifetime, multiplier)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerUpFixed(int bonus, ITargetSelector direction)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new UnitBoundEffectLifetime(u);
                    var effect = new ModifyDamageEffect(u, effectLifetime, bonus)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreateProtection(int equipmentLevel,
            ITargetSelector direction, float equipmentMultiplier)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new UnitBoundEffectLifetime(u);
                    var effectMultiplier = (equipmentLevel + 1) * equipmentMultiplier;
                    var effect = new ProtectionEffect(u, effectLifetime, effectMultiplier)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        /// <summary>
        /// Create protection rule with single turn duration.
        /// </summary>
        public static EffectRule CreateProtection(SkillSid sid, ITargetSelector direction,
            float multiplier)
        {
            return CreateProtection(sid, direction, 1, multiplier);
        }

        /// <summary>
        /// Create protection rule with single turn duration to yourself.
        /// </summary>
        public static EffectRule CreateProtection(SkillSid sid, float multiplier)
        {
            return CreateProtection(sid, SkillDirection.Self, 1, multiplier);
        }

        public static EffectRule CreateProtection(SkillSid sid, ITargetSelector direction, int duration,
            float multiplier, IReadOnlyCollection<IEffectCondition>? imposeConditions = default)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var durationEffectLifetime = new DurationEffectLifetime(compensationDuration);
                    var effect = new ProtectionEffect(u, durationEffectLifetime, multiplier)
                    {
                        Visualization = EffectVisualizations.Protection,
                        ImposeConditions = imposeConditions
                    };

                    return effect;
                })
            };
        }

        private static EffectDuration GetCompensatedDuration(ITargetSelector direction, int duration)
        {
            return new EffectDuration(duration, direction == SkillDirection.Self);
        }
    }
}
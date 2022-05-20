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

        public static EffectRule CreateDamage(SkillSid sid, SkillDirection direction)
        {
            return CreateDamage(sid, direction, 1f);
        }

        public static EffectRule CreateDamage(SkillSid sid, SkillDirection direction, float multiplier)
        {
            return CreateDamage(sid, direction, multiplier, scatter: 0.1f);
        }

        public static EffectRule CreateDamage(SkillSid sid, SkillDirection direction, float multiplier, float scatter)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplierBonus = u.Unit.GetEquipmentDamageMultiplierBonus(sid);

                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = multiplier * (1 + equipmentMultiplierBonus),
                        Scatter = scatter
                    };

                    return res;
                })
            };
        }

        public static EffectRule CreatePeriodicDamage(SkillSid sid, int duration, SkillDirection direction)
        {
            return CreatePeriodicDamage(sid, power: 1f, duration, direction);
        }

        public static EffectRule CreatePeriodicDamage(SkillSid sid, float power, int duration, SkillDirection direction)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new PeriodicDamageEffect(u, compensationDuration)
                    {
                        PowerMultiplier = power,
                        Visualization = EffectVisualizations.Damage
                    };

                    return effect;
                })
            };
        }

        public static EffectRule CreatePeriodicHealing(SkillSid sid, float power, int duration,
            SkillDirection direction)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplierBonus = u.Unit.GetEquipmentHealMultiplierBonus(sid);

                    var effect = new PeriodicHealEffect(u, compensationDuration)
                    {
                        PowerMultiplier = power * (1 + equipmentMultiplierBonus),
                        Visualization = EffectVisualizations.Healing
                    };

                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerDown(SkillSid sid, SkillDirection direction, int duration)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new IncreaseAttackEffect(u, duration: compensationDuration, bonus: -u.Unit.Support)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerUp(int equipmentLevel, SkillDirection direction = SkillDirection.AllFriendly, float equipmentMuliplier = 0.5f)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effectLifetime = new UnitBoundEffectLifetime(u.Unit);
                    var effectMultiplier = (equipmentLevel + 1) * equipmentMuliplier;
                    var effect = new IncreaseDamagePercentEffect(u, effectLifetime, effectMultiplier)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerUp(SkillSid sid, SkillDirection direction)
        {
            return CreatePowerUp(sid, direction, duration: 1);
        }

        public static EffectRule CreatePowerUp(SkillSid sid, SkillDirection direction, int duration)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new IncreaseAttackEffect(u, duration: compensationDuration, bonus: u.Unit.Support)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        private static int GetCompensatedDuration(SkillDirection direction, int duration)
        {
            var compensationDuration = duration;
            if (direction == SkillDirection.Self)
            {
                compensationDuration = duration + 1;
            }

            return compensationDuration;
        }

        /// <summary>
        /// Create protection rule with single turn duration.
        /// </summary>
        public static EffectRule CreateProtection(SkillSid sid, SkillDirection direction,
            float multiplier)
        {
            return CreateProtection(sid, direction, 1, multiplier);
        }

        /// <summary>
        /// Create protection rule with single turn duration to youself.
        /// </summary>
        public static EffectRule CreateProtection(SkillSid sid, float multiplier)
        {
            return CreateProtection(sid, SkillDirection.Self, 1, multiplier);
        }

        public static EffectRule CreateProtection(SkillSid sid, SkillDirection direction, int duration,
            float multiplier)
        {
            var compensationDuration = GetCompensatedDuration(direction, duration);

            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    // TODO +1 is durty fix
                    var effect = new DecreaseDamageEffect(u, compensationDuration, multiplier)
                    {
                        Visualization = EffectVisualizations.Protection
                    };

                    return effect;
                })
            };
        }
    }
}
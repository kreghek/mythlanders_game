using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets
{
    internal static class SkillRuleFactory
    {
        public static EffectRule CreatePeriodicHealing(SkillSid sid, float power, int duration, SkillDirection direction)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new PeriodicHealEffect(u, duration)
                    {
                        PowerMultiplier = power,
                        Visualization = EffectVisualizations.Healing
                    };

                    return effect;
                })
            };
        }

        public static EffectRule CreatePeriodicDamage(SkillSid sid, int duration, SkillDirection direction)
        {
            return CreatePeriodicDamage(sid, power: 1f, duration, direction);
        }

        public static EffectRule CreatePeriodicDamage(SkillSid sid, float power, int duration, SkillDirection direction)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new PeriodicDamageEffect(u, duration)
                    {
                        PowerMultiplier = power,
                        Visualization = EffectVisualizations.Damage
                    };

                    return effect;
                })
            };
        }

        public static EffectRule CreatePowerUp(SkillSid sid, SkillDirection direction)
        {
            return CreatePowerUp(sid, duration: 1, direction);
        }

        public static EffectRule CreatePowerUp(SkillSid sid, int duration, SkillDirection direction)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new IncreaseAttackEffect(u, duration: duration, bonus: u.Unit.Support)
                    {
                        Visualization = EffectVisualizations.PowerUp
                    };
                    return effect;
                })
            };
        }

        public static EffectRule CreateProtection(SkillSid sid, float multiplier,
            SkillDirection direction)
        {
            return CreateProtection(sid, 1, multiplier, direction);
        }

        public static EffectRule CreateProtection(SkillSid sid, int duration, float multiplier,
            SkillDirection direction)
        {
            return new EffectRule
            {
                Direction = direction,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DecreaseDamageEffect(u, duration, multiplier)
                    {
                        Visualization = EffectVisualizations.Protection
                    };

                    return effect;
                })
            };
        }
    }
}
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal interface ISkillEffectDrawer
    {
        bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position);
    }

    internal static class SkillEffectDrawerHelper
    {
        internal static string GetLocalized(SkillDirection direction)
        {
            switch (direction)
            {
                case SkillDirection.Target:
                    return UiResource.SkillDirectionTargetText;
                case SkillDirection.Self:
                    return UiResource.SkillDirectionSelfText;
                default:
                    throw new ArgumentException($"{direction} is not known direction value.");
            }
        }
    }

    internal class DamageEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public DamageEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not DamageEffect attackEffect)
            {
                return false;
            }

            var damage = attackEffect.CalculateDamage();

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(rule.Direction);
            spriteBatch.DrawString(_font,
                string.Format(UiResource.DamageEffectRuleText, damage.Min, damage.Max, ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }

    internal class HealEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public HealEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not HealEffect healEffect)
            {
                return false;
            }

            var heal = healEffect.CalculateHeal();

            spriteBatch.DrawString(_font,
                string.Format(UiResource.HealEffectRuleText, heal.Min, heal.Max, rule.Direction),
                position, Color.Wheat);

            return true;
        }
    }

    internal class HealOverTimeEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public HealOverTimeEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not PeriodicHealEffect periodicHealEffect)
            {
                return false;
            }

            var heal = periodicHealEffect.CalculateHeal();

            spriteBatch.DrawString(_font,
                string.Format(UiResource.HealOverTimeEffectRuleText, heal.Min, heal.Max, periodicHealEffect.Duration,
                    periodicHealEffect.Target),
                position, Color.Wheat);

            return true;
        }
    }
}
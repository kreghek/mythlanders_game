﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
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
                position,
                Color.Wheat);

            return true;
        }
    }

    internal class ExchangeSlotEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public ExchangeSlotEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not ExchangeSlotEffect attackEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(rule.Direction);
            spriteBatch.DrawString(_font,
                "EXCHANGE SLOT",
                position,
                Color.Wheat);

            return true;
        }
    }
}
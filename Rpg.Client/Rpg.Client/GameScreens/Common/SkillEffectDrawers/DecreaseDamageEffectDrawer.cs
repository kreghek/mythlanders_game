using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class DecreaseDamageEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public DecreaseDamageEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, SkillDirection direction, Vector2 position)
        {
            if (effectToDisplay is not ProtectionEffect decreaseDamageEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

            var percentage = (int)Math.Round(decreaseDamageEffect.Multiplier * 100, 0, MidpointRounding.AwayFromZero);
            spriteBatch.DrawString(_font,
                string.Format(UiResource.DecreaseDamageEffectRuleText, percentage,
                    decreaseDamageEffect.EffectLifetime.GetTextDescription(),
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
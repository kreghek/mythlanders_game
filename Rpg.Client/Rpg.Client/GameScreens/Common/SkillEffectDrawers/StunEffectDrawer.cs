using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class StunEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public StunEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not StunEffect stunEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(rule.Direction);

            spriteBatch.DrawString(_font,
                string.Format(UiResource.StunEffectRuleText, stunEffect.Duration,
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
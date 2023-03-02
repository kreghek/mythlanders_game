using Client;
using Client.Core.Skills;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class StunEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public StunEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not StunEffect stunEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

            spriteBatch.DrawString(_font,
                string.Format(UiResource.StunEffectRuleText, stunEffect.EffectLifetime.GetTextDescription(),
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
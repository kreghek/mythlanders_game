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

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not DecreaseDamageEffect decreaseDamageEffect)
            {
                return false;
            }

            spriteBatch.DrawString(_font,
                string.Format(UiResource.DecreaseDamageEffectRuleText, decreaseDamageEffect.Multiplier * 100, decreaseDamageEffect.Duration,
                    decreaseDamageEffect.Target),
                position, Color.Wheat);

            return true;
        }
    }
}
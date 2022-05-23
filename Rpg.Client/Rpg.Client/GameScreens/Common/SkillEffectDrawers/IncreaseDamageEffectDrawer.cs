using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class IncreaseDamageEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public IncreaseDamageEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not IncreaseAttackEffect increaseDamageEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(rule.Direction);

            spriteBatch.DrawString(_font,
                string.Format(UiResource.IncreaseDamageEffectRuleText, increaseDamageEffect.Bonus,
                    increaseDamageEffect.EffectLifetime.GetTextDescription(),
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
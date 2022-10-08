using Client;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class ModifyDamageEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public ModifyDamageEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not ModifyDamageEffect increaseDamageEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

            var textTemplate = increaseDamageEffect.Bonus > 0
                ? UiResource.IncreaseDamageEffectRuleText
                : UiResource.DecreseDamageEffectRuleText;

            spriteBatch.DrawString(_font,
                string.Format(textTemplate, increaseDamageEffect.Bonus,
                    increaseDamageEffect.EffectLifetime.GetTextDescription(),
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
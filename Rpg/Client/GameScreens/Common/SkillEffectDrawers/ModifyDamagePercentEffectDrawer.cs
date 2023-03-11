using Client.GameScreens.Common.SkillEffectDrawers;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class ModifyDamagePercentEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public ModifyDamagePercentEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
            Vector2 position)
        {
            //if (effectToDisplay is not ModifyDamagePercentEffect increaseDamageEffect)
            //{
            //    return false;
            //}

            //var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

            //spriteBatch.DrawString(_font,
            //    string.Format(UiResource.IncreaseDamagePercentEffectRuleText, increaseDamageEffect.Multiplier,
            //        increaseDamageEffect.EffectLifetime.GetTextDescription(),
            //        ruleDirectionText),
            //    position, Color.Wheat);

            return true;
        }
    }
}
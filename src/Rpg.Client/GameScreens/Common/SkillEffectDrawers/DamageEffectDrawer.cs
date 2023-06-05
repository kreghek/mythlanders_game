using Microsoft.Xna.Framework;
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

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not DamageEffect attackEffect)
            {
                return false;
            }

            var damage = attackEffect.CalculateDamage();

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);
            spriteBatch.DrawString(_font,
                string.Format(UiResource.DamageEffectRuleText, damage.Min, damage.Max, ruleDirectionText),
                position,
                Color.Wheat);

            return true;
        }
    }
}
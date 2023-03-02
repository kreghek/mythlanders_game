using Client;
using Client.Core.Skills;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class PeriodicDamageEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public PeriodicDamageEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not PeriodicDamageEffect attackEffect)
            {
                return false;
            }

            var damage = attackEffect.CalculateDamage();

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);
            spriteBatch.DrawString(_font,
                string.Format(UiResource.PeriodicDamageEffectRuleText, damage.Min, damage.Max, ruleDirectionText,
                    attackEffect.EffectLifetime.GetTextDescription()),
                position, Color.Wheat);

            return true;
        }
    }
}
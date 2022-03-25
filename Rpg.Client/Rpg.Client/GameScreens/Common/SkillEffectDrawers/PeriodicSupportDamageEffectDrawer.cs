using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class PeriodicSupportDamageEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public PeriodicSupportDamageEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not PeriodicSupportAttackEffect attackEffect)
            {
                return false;
            }

            var damage = attackEffect.CalculateRoundDamage();

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(rule.Direction);
            spriteBatch.DrawString(_font,
                string.Format(UiResource.PeriodicDamageEffectRuleText, damage.Min, damage.Max, ruleDirectionText,
                    attackEffect.Duration),
                position, Color.Wheat);

            return true;
        }
    }
}
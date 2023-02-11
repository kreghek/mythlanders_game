using Client;
using Client.Core.Skills;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class LifeDrawEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public LifeDrawEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not LifeDrawEffect lifeDrawEffect)
            {
                return false;
            }

            var damage = lifeDrawEffect.CalculateDamage();
            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

            spriteBatch.DrawString(_font,
                string.Format(UiResource.LifeDrawEffectRuleText, damage.Min, damage.Max,
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
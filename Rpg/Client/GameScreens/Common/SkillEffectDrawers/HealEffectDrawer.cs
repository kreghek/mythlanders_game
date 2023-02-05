using Client;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class HealEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public HealEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not HealEffect healEffect)
            {
                return false;
            }

            var heal = healEffect.CalculateHeal();

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

            spriteBatch.DrawString(_font,
                string.Format(UiResource.HealEffectRuleText, heal.Min, heal.Max, ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
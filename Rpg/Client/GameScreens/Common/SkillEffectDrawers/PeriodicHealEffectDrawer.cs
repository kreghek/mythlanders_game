using Client;
using Client.Core.Skills;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class PeriodicHealEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public PeriodicHealEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not PeriodicHealEffect periodicHealEffect)
            {
                return false;
            }

            var heal = periodicHealEffect.CalculateHeal();
            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

            spriteBatch.DrawString(_font,
                string.Format(UiResource.PeriodicHealEffectRuleText, heal.Min, heal.Max,
                    ruleDirectionText, periodicHealEffect.EffectLifetime.GetTextDescription()),
                position, Color.Wheat);

            return true;
        }
    }
}
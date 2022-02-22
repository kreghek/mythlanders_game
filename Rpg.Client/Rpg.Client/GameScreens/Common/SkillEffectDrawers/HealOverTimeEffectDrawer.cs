using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class HealOverTimeEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public HealOverTimeEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, EffectRule rule, Vector2 position)
        {
            if (effectToDisplay is not PeriodicHealEffect periodicHealEffect)
            {
                return false;
            }

            var heal = periodicHealEffect.CalculateHeal();

            spriteBatch.DrawString(_font,
                string.Format(UiResource.HealOverTimeEffectRuleText, heal.Min, heal.Max, periodicHealEffect.Duration,
                    periodicHealEffect.Target),
                position, Color.Wheat);

            return true;
        }
    }
}
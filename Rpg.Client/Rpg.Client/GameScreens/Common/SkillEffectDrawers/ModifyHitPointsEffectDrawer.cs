using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class ModifyHitPointsEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public ModifyHitPointsEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, object effectToDisplay, ITargetSelector direction, Vector2 position)
        {
            if (effectToDisplay is not HitPointModifyEffect hpModifyEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);
            var textTemplate = hpModifyEffect.Modifier > 0 ? UiResource.IncreaseHitPointsPercentEffectRuleText : UiResource.DecreaseHitPointsPercentEffectRuleText;

            spriteBatch.DrawString(_font,
                string.Format(textTemplate,
                    SkillEffectDrawerHelper.GetPercent(hpModifyEffect.Modifier),
                    hpModifyEffect.EffectLifetime.GetTextDescription(),
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
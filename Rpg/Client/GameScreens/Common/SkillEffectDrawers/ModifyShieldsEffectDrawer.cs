using Client;
using Client.Core.Skills;
using Client.GameScreens.Common.SkillEffectDrawers;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;

using ITargetSelector = Client.Core.Skills.ITargetSelector;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal class ModifyShieldsEffectDrawer : ISkillEffectDrawer
    {
        private readonly SpriteFont _font;

        public ModifyShieldsEffectDrawer(SpriteFont font)
        {
            _font = font;
        }

        public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
            Vector2 position)
        {
            if (effectToDisplay is not ShieldPointModifyEffect shieldModifyEffect)
            {
                return false;
            }

            var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);
            var textTemplate = shieldModifyEffect.Modifier > 0
                ? UiResource.IncreaseShieldsPercentEffectRuleText
                : UiResource.DecreaseShieldsPercentEffectRuleText;

            spriteBatch.DrawString(_font,
                string.Format(textTemplate,
                    SkillEffectDrawerHelper.GetPercent(shieldModifyEffect.Modifier),
                    shieldModifyEffect.EffectLifetime.GetTextDescription(),
                    ruleDirectionText),
                position, Color.Wheat);

            return true;
        }
    }
}
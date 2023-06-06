using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal class ProtectionEffectDrawer : ISkillEffectDrawer
{
    private readonly SpriteFont _font;

    public ProtectionEffectDrawer(SpriteFont font)
    {
        _font = font;
    }

    public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
        Vector2 position)
    {
        //if (effectToDisplay is not ProtectionEffect protectionEffect)
        //{
        //    return false;
        //}

        //var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

        //var percentage = (int)Math.Round(protectionEffect.Multiplier * 100, 0, MidpointRounding.AwayFromZero);
        //var descriptionText = string.Format(UiResource.ProtectionEffectRuleText, percentage,
        //    protectionEffect.EffectLifetime.GetTextDescription(),
        //    ruleDirectionText);

        //if (protectionEffect.ImposeConditions is not null)
        //{
        //    foreach (var imposeCondition in protectionEffect.ImposeConditions)
        //    {
        //        descriptionText += " " + imposeCondition.GetDescription();
        //    }
        //}

        //spriteBatch.DrawString(_font,
        //    descriptionText,
        //    position, Color.Wheat);

        return true;
    }
}
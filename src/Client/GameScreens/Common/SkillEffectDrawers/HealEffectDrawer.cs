using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal class HealEffectDrawer : ISkillEffectDrawer
{
    private readonly SpriteFont _font;

    public HealEffectDrawer(SpriteFont font)
    {
        _font = font;
    }

    public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
        Vector2 position)
    {
        //if (effectToDisplay is not HealEffect healEffect)
        //{
        //    return false;
        //}

        //var heal = healEffect.CalculateHeal();

        //var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

        //spriteBatch.DrawString(_font,
        //    string.Format(UiResource.HealEffectRuleText, heal.Min, heal.Max, ruleDirectionText),
        //    position, Color.Wheat);

        return true;
    }
}
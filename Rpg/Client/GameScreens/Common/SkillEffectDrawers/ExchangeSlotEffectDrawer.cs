using Client.Core.Skills;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal class ExchangeSlotEffectDrawer : ISkillEffectDrawer
{
    private readonly SpriteFont _font;

    public ExchangeSlotEffectDrawer(SpriteFont font)
    {
        _font = font;
    }

    public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
        Vector2 position)
    {
        //if (effectToDisplay is not ExchangeSlotEffect exchangeEffect)
        //{
        //    return false;
        //}

        //var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);
        //spriteBatch.DrawString(_font,
        //    "EXCHANGE SLOT",
        //    position,
        //    Color.Wheat);

        return true;
    }
}
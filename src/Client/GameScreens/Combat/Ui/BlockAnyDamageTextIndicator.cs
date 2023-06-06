using Client;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class BlockAnyDamageTextIndicator : TextIndicatorBase
{
    public BlockAnyDamageTextIndicator(Vector2 startPosition, SpriteFont font) : base(startPosition, font)
    {
    }

    protected override Color GetColor()
    {
        return Color.LightGray;
    }

    protected override string GetText()
    {
        return UiResource.IndicatorBlockAnyDamage;
    }
}
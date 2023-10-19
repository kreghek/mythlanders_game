using Client.Assets.CombatVisualEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class BlockAnyDamageTextIndicator : TextIndicatorBase
{
    public BlockAnyDamageTextIndicator(Vector2 startPosition, SpriteFont font, int stackIndex) : base(startPosition + new Vector2(stackIndex * 20, 0), font)
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
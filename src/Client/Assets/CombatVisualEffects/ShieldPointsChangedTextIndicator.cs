using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.CombatVisualEffects;

internal class ShieldPointsChangedTextIndicator : TextIndicatorBase
{
    private readonly int _amount;
    private readonly StatChangeDirection _direction;

    public ShieldPointsChangedTextIndicator(int amount, StatChangeDirection direction,
        Vector2 startPosition,
        SpriteFont font, int stackIndex) : base(startPosition + new Vector2(0, stackIndex * 20), font)
    {
        _amount = amount;
        _direction = direction;
    }

    protected override Color GetColor()
    {
        return _direction == StatChangeDirection.Positive ? Color.LightGray : Color.LightGray;
    }

    protected override string GetText()
    {
        if (_amount > 0 && _direction == StatChangeDirection.Positive)
        {
            return string.Format(UiResource.CombatIndicatorPositiveSpTemplate, _amount);
        }

        return string.Format(UiResource.CombatIndicatorNegativeSpTemplate, _amount);
    }
}
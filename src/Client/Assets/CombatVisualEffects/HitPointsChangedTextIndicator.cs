using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.CombatVisualEffects;

internal class HitPointsChangedTextIndicator : TextIndicatorBase
{
    private readonly int _amount;
    private readonly StatChangeDirection _direction;

    public HitPointsChangedTextIndicator(int amount, StatChangeDirection direction,
        Vector2 startPosition,
        SpriteFont font, int stackIndex) : base(startPosition + new Vector2(0, stackIndex * 20), font)
    {
        _amount = amount;
        _direction = direction;
    }

    protected override Color GetColor()
    {
        return _direction == StatChangeDirection.Positive ? Color.LightGreen : Color.Red;
    }

    protected override string GetText()
    {
        if (_amount > 0 && _direction == StatChangeDirection.Positive)
        {
            return string.Format(UiResource.CombatIndicatorPositiveHpTemplate, _amount);
        }

        return string.Format(UiResource.CombatIndicatorNegativeHpTemplate, _amount);
    }
}
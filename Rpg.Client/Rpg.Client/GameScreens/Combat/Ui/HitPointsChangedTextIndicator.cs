using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class HitPointsChangedTextIndicator : TextIndicatorBase
    {
        private readonly int _amount;
        private readonly int? _shieldValue;
        private readonly HitPointsChangeDirection _direction;

        public HitPointsChangedTextIndicator(int amount, int? shieldValue, HitPointsChangeDirection direction,
            Vector2 startPosition,
            SpriteFont font, int stackIndex) : base(startPosition + new Vector2(stackIndex * 20, 0), font)
        {
            _amount = amount;
            _shieldValue = shieldValue;
            _direction = direction;
        }

        protected override Color GetColor()
        {
            return _direction == HitPointsChangeDirection.Positive ? Color.LightGreen : Color.Red;
        }

        protected override string GetText()
        {
            if (_amount > 0 && _direction == HitPointsChangeDirection.Positive)
            {
                return $"+{_amount}";
            }

            return (_shieldValue is not null ? _shieldValue.Value.ToString() + "s" : string.Empty) + _amount.ToString();
        }
    }
}
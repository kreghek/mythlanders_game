using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class ShieldPointsChangedTextIndicator : TextIndicatorBase
    {
        private readonly int _amount;
        private readonly HitPointsChangeDirection _direction;

        public ShieldPointsChangedTextIndicator(int amount, HitPointsChangeDirection direction,
            Vector2 startPosition,
            SpriteFont font, int stackIndex) : base(startPosition + new Vector2(stackIndex * 20, 0), font)
        {
            _amount = amount;
            _direction = direction;
        }

        protected override Color GetColor()
        {
            return _direction == HitPointsChangeDirection.Positive ? Color.LightGray : Color.LightGray;
        }

        protected override string GetText()
        {
            if (_amount > 0 && _direction == HitPointsChangeDirection.Positive)
            {
                return $"+{_amount}";
            }

            return _amount.ToString();
        }
    }
}
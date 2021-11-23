using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class HitPointsChangedComponent : DisapearingTextComponent
    {
        private readonly int _amount;
        private readonly HitPointsChangeDirection _direction;

        public HitPointsChangedComponent(int amount, HitPointsChangeDirection direction, Vector2 startPosition, SpriteFont font) : base(startPosition, font)
        {
            _amount = amount;
            _direction = direction;
        }

        protected override Color GetColor()
        {
            return _direction == HitPointsChangeDirection.Positive ? Color.LightGreen : Color.Red;
        }

        protected override string GetText()
        {
            if (_amount > 0)
            {
                return $"+{_amount}";
            }

            return _amount.ToString();
        }
    }
}
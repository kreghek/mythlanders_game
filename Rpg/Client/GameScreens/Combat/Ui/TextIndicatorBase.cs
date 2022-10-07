using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal abstract class TextIndicatorBase : EwarRenderableBase
    {
        private const float LIFETIME_SECONDS = 2;

        private readonly SpriteFont _font;
        private readonly Vector2 _targetPosition;
        private float _lifetimeCounter;
        private Vector2 _position;

        public TextIndicatorBase(Vector2 startPosition, SpriteFont font)
        {
            _position = startPosition + Vector2.UnitY * -64;
            _targetPosition = _position + Vector2.UnitY * -64;
            _font = font;
            _lifetimeCounter = LIFETIME_SECONDS;
        }

        public override void Update(GameTime gameTime)
        {
            if (_lifetimeCounter <= 0)
            {
                Parent?.RemoveChild(this);

                return;
            }

            var elapsedSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _lifetimeCounter -= elapsedSec;

            var t = 1 - _lifetimeCounter / LIFETIME_SECONDS;

            _position = Vector2.Lerp(_position, _targetPosition, t);

            base.Update(gameTime);
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zIndex)
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    spriteBatch.DrawString(_font, GetText(), _position + new Vector2(x, y), Color.DarkCyan);
                }
            }

            spriteBatch.DrawString(_font, GetText(), _position, GetColor());
        }

        protected abstract Color GetColor();

        protected abstract string GetText();
    }
}
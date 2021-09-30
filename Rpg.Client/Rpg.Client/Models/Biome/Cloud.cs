using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models.Biome
{
    internal sealed class Cloud
    {
        private const double DURATION_SECONDS = 30;
        private readonly Vector2 _endPosition;
        private readonly double _speed;
        private readonly Vector2 _startPosition;
        private readonly Texture2D _texture;
        private readonly int _textureIndex;

        private Vector2 _currentPosition;
        private double _lifetimeCounter;

        public Cloud(Texture2D texture, int textureIndex, Vector2 startPosition, Vector2 endPosition, double speed)
        {
            _texture = texture;
            _textureIndex = textureIndex;
            _startPosition = startPosition;
            _endPosition = endPosition;
            _speed = speed;
            _lifetimeCounter = DURATION_SECONDS;
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = _currentPosition;
            spriteBatch.Draw(_texture, new Rectangle(position.ToPoint(), new Point(64, 64)),
                new Rectangle(_textureIndex * 64, 0, 64, 64), Color.Lerp(Color.White, Color.Transparent, 0.25f));
        }

        public void DrawShadows(SpriteBatch spriteBatch)
        {
            var position = _currentPosition + Vector2.UnitY * 50;
            spriteBatch.Draw(_texture, new Rectangle(position.ToPoint(), new Point(64, 64)),
                new Rectangle(_textureIndex * 64, 0, 64, 64), Color.Lerp(Color.Black, Color.Transparent, 0.5f));
        }

        public void Update(GameTime gameTime)
        {
            _lifetimeCounter -= gameTime.ElapsedGameTime.TotalSeconds * _speed;
            if (_lifetimeCounter <= 0)
            {
                IsDestroyed = true;
            }
            else
            {
                var t = _lifetimeCounter / DURATION_SECONDS;
                var invertedT = 1 - t;
                _currentPosition = Vector2.Lerp(_startPosition, _endPosition, (float)invertedT);
            }
        }
    }
}
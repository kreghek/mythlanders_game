using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects.Background
{
    internal sealed class BackgroundCloud : IBackgroundObject
    {
        private const double DURATION_SECONDS = 160;

        private static readonly Random _random = new Random();
        private readonly Sprite _cloudSprite;
        private readonly Vector2 _endPosition;
        private readonly double _speed;
        private readonly Vector2 _startPosition;

        private readonly Texture2D _texture;
        private readonly int _textureIndex;
        private Vector2 _currentPosition;

        private double _lifetimeCounter;

        public BackgroundCloud(Texture2D texture, int textureIndex, Vector2 startPosition, Vector2 endPosition,
            double speed)
        {
            _texture = texture;
            _textureIndex = textureIndex;
            _startPosition = startPosition;
            _endPosition = endPosition;
            _speed = speed;

            _lifetimeCounter = DURATION_SECONDS * _random.NextDouble();

            _cloudSprite = new Sprite(_texture)
            {
                Color = Color.Lerp(Color.White, Color.Transparent, 0.25f)
            };
            _cloudSprite.SourceRectangle = new Rectangle(0, _textureIndex * 32, 128, 32);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _cloudSprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _lifetimeCounter -= gameTime.ElapsedGameTime.TotalSeconds * _speed;
            if (_lifetimeCounter <= 0)
            {
                _lifetimeCounter = DURATION_SECONDS;
            }
            else
            {
                var t = _lifetimeCounter / DURATION_SECONDS;
                var invertedT = 1 - t;
                _currentPosition = Vector2.Lerp(_startPosition, _endPosition, (float)invertedT);
            }

            _cloudSprite.Position = _currentPosition;
        }
    }
}
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Biome
{
    internal sealed class Cloud
    {
        private const double DURATION_SECONDS = 30;

        private static readonly Random _random = new Random();

        private readonly Sprite _cloudSprite;
        private readonly Vector2 _endPosition;
        private readonly Sprite _shadowSprite;
        private readonly double _speed;
        private readonly Vector2 _startPosition;
        private readonly Texture2D _texture;
        private readonly int _textureIndex;

        private Vector2 _currentPosition;
        private double _lifetimeCounter;

        public Cloud(Texture2D texture, int textureIndex, Vector2 startPosition, Vector2 endPosition, double speed,
            bool screenInitStage)
        {
            _texture = texture;
            _textureIndex = textureIndex;
            _startPosition = startPosition;
            _endPosition = endPosition;
            _speed = speed;
            if (screenInitStage)
            {
                _lifetimeCounter = DURATION_SECONDS * _random.NextDouble();
            }
            else
            {
                _lifetimeCounter = DURATION_SECONDS;
            }

            _cloudSprite = new Sprite(_texture)
            {
                Color = Color.Lerp(Color.White, Color.Transparent, 0.25f)
            };
            _shadowSprite = new Sprite(_texture)
            {
                Color = Color.Lerp(Color.Black, Color.Transparent, 0.5f)
            };
        }

        public bool IsDestroyed { get; private set; }

        public Sprite GetShadow()
        {
            return _shadowSprite;
        }

        public Sprite GetSprite()
        {
            return _cloudSprite;
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

            _cloudSprite.Position = _currentPosition;
            _cloudSprite.SourceRectangle = new Rectangle(_textureIndex * 64, 0, 64, 64);

            _shadowSprite.Position = _currentPosition + Vector2.UnitY * 50;
            _shadowSprite.SourceRectangle = new Rectangle(_textureIndex * 64, 0, 64, 64);
        }
    }
}
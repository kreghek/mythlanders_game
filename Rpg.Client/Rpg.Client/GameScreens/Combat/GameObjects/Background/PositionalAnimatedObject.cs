using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal sealed class PositionalAnimatedObject : IBackgroundObject
    {
        private const int FPS = 2;
        private readonly Sprite _sprite;

        private double _frameCounter;
        private int _frameIndex;

        public PositionalAnimatedObject(Texture2D texture, Vector2 position)
        {
            _sprite = new Sprite(texture)
            {
                Position = position
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.SourceRectangle = new Rectangle(0, _frameIndex * 256, 256, 256);
            _sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_frameCounter >= 1f / FPS)
            {
                _frameIndex++;
                _frameCounter = 0;

                if (_frameIndex >= 4)
                {
                    _frameIndex = 0;
                }
            }
        }
    }

    internal sealed class WeatherAnimatedObject : IBackgroundObject
    {
        private ParticleSystem _particleSystem;

        public WeatherAnimatedObject(Texture2D texture, Rectangle sourceRect)
        {
            var particleGenerator = new WeatherParticleGenerator(new[] { texture });
            _particleSystem = new ParticleSystem(Vector2.Zero, particleGenerator);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _particleSystem.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _particleSystem.Update(gameTime);
        }
    }
}
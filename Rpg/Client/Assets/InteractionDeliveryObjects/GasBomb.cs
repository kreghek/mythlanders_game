using System;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.InteractionDeliveryObjects
{
    internal sealed class GasBomb : IInteractionDelivery
    {
        private const double DURATION_SECONDS = 0.3;
        private const double FRAME_RATE = 1f / (8f * 3);

        private const int FRAME_COUNT = 4;

        private readonly AnimationBlocker? _blocker;
        private readonly Vector2 _endPosition;
        private readonly Sprite _graphics;
        private readonly ParticleSystem _particleSystem;
        private readonly Vector2 _startPosition;
        private double _counter;
        private double _counter2;
        private double _frameCounter;
        private int _frameIndex;

        public GasBomb(Vector2 startPosition, Vector2 endPosition, GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker)
        {
            _graphics = new Sprite(contentStorage.GetBulletGraphics())
            {
                Position = startPosition,
                SourceRectangle = new Rectangle(0, 0, 64, 32),
                Color = Color.LimeGreen
            };

            _startPosition = startPosition;
            _endPosition = endPosition;
            _blocker = blocker;

            var particleGenerator = new ExplosionParticleGenerator(new[] { contentStorage.GetParticlesTexture() },
                new Rectangle(0, 64 + 32, 32, 32));
            _particleSystem = new ParticleSystem(_startPosition, particleGenerator);
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);

            if (_counter2 > 0)
            {
                _particleSystem.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            if (_counter < DURATION_SECONDS)
            {
                _counter += gameTime.ElapsedGameTime.TotalSeconds;
                _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;

                if (_frameCounter >= FRAME_RATE)
                {
                    _frameCounter = 0;
                    _frameIndex++;

                    if (_frameIndex > FRAME_COUNT - 1)
                    {
                        _frameIndex = 0;
                    }
                }

                var t = _counter / DURATION_SECONDS;
                _graphics.Position = Vector2.Lerp(_startPosition, _endPosition, (float)t) +
                                     Vector2.UnitY * (float)Math.Sin(t * Math.PI * 2) * 128;
                _graphics.SourceRectangle = new Rectangle(0, 32 * _frameIndex, 64, 32);
                _graphics.Rotation = MathF.Atan2(_endPosition.Y - _startPosition.Y, _endPosition.X - _startPosition.X);
            }
            else
            {
                if (_counter2 < DURATION_SECONDS)
                {
                    _counter2 += gameTime.ElapsedGameTime.TotalSeconds;
                    _particleSystem.MoveEmitter(_graphics.Position);
                    _particleSystem.Update(gameTime);
                }
                else
                {
                    if (!IsDestroyed)
                    {
                        IsDestroyed = true;
                        _blocker?.Release();
                        InteractionPerformed?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler? InteractionPerformed;
    }
}
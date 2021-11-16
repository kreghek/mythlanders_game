using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class SymbolObject : IInteractionDelivery
    {
        private const double FRAMERATE = 1f / 8f;
        private const int SYMBOL_SIZE = 128;

        private const int FRAME_COUNT = 18 + 8 * 3;
        private readonly AnimationBlocker? _blocker;
        private readonly Sprite _graphics;
        private readonly Vector2 _targetPosition;
        private readonly GameObjectContentStorage _contentStorage;
        private double _counter;
        private double _frameCounter;
        private int _frameIndex;
        private ParticleSystem _particleSystem;

        public SymbolObject(Vector2 targetPosition, GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker)
        {
            _graphics = new Sprite(contentStorage.GetSymbolSprite());

            _targetPosition = targetPosition;
            _contentStorage = contentStorage;
            _blocker = blocker;
            _graphics.Position = _targetPosition;
            _graphics.SourceRectangle = new Rectangle(0, 0, SYMBOL_SIZE, SYMBOL_SIZE);

            var particleTextures = new[] { contentStorage.GetParticlesTexture() };
            var particleGenerator = new MothParticleGenerator(particleTextures);
            _particleSystem = new ParticleSystem(targetPosition, particleGenerator);
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);
            _particleSystem.Draw(spriteBatch);
        }

        private int _stageIndex = 0;

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_stageIndex == 0)
            {
                _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;

                if (_frameCounter >= FRAMERATE)
                {
                    _frameCounter = 0;
                    _frameIndex++;

                    if (_frameIndex > FRAME_COUNT - 1)
                    {
                        _frameIndex = FRAME_COUNT - 1;
                        if (!IsDestroyed)
                        {
                            var explosionParticleGenerator = new ExplosionParticleGenerator(new[] { _contentStorage.GetParticlesTexture() });
                            _particleSystem = new ParticleSystem(_targetPosition, explosionParticleGenerator);
                            _stageIndex++;
                            _counter = 0;
                        }
                    }
                }

                var normalizedFrameIndex = _frameIndex;
                if (_frameIndex >= 18)
                {
                    normalizedFrameIndex = 16 + _frameIndex % 2;
                }

                _graphics.SourceRectangle = new Rectangle(SYMBOL_SIZE * (normalizedFrameIndex % 4), SYMBOL_SIZE * (normalizedFrameIndex / 4),
                    SYMBOL_SIZE, SYMBOL_SIZE);

                _particleSystem.Update(gameTime);
            }
            else if (_stageIndex == 1)
            {
                _particleSystem.Update(gameTime);

                if (_counter >= 1)
                {
                    if (!_explosionExecuted)
                    {
                        _explosionExecuted = true;
                        _blocker?.Release();
                        IsDestroyed = true;
                    }
                }
            }
        }

        private bool _explosionExecuted;
    }
}
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.InteractionDeliveryObjects
{
    internal sealed class EnergoArrowProjectile : IInteractionDelivery
    {
        private const double LIFETIME_DURATION_SECONDS = 0.3;
        private const int FPS = 8 * 3;
        private const double FRAMERATE = 1f / FPS;

        private const int FRAME_COUNT = 4;

        private readonly AnimationBlocker? _blocker;
        private readonly Vector2 _endPosition;
        private readonly Sprite _graphics;
        private readonly Action<ICombatUnit>? _interaction;
        private readonly double _lifetimeDuration;
        private readonly Vector2 _startPosition;
        private readonly ParticleSystem _tailParticleSystem;
        private readonly ICombatUnit? _targetCombatUnit;
        private double _lifetimeCounter;
        private double _frameCounter;
        private int _frameIndex;

        private readonly IAnimationFrameSet _frameSet;

        public EnergoArrowProjectile(Vector2 startPosition,
            Vector2 endPosition,
            GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker,
            ICombatUnit? targetCombatUnit = null,
            Action<ICombatUnit>? interaction = null,
            double lifetimeDuration = LIFETIME_DURATION_SECONDS)
        {
            _graphics = new Sprite(contentStorage.GetBulletGraphics())
            {
                Position = startPosition,
                SourceRectangle = new Rectangle(0, 0, 64, 32)
            };

            _startPosition = startPosition;
            _endPosition = endPosition;
            _blocker = blocker;
            _targetCombatUnit = targetCombatUnit;
            _interaction = interaction;
            _lifetimeDuration = lifetimeDuration;
            var particleGenerator = new TailParticleGenerator(new[] { contentStorage.GetParticlesTexture() });
            _tailParticleSystem = new ParticleSystem(_startPosition, particleGenerator);

            _frameSet = AnimationFrameSetFactory.CreateSequential(0, frameCount:4, speedMultiplicator: FPS, frameWidth: 64, frameHeight: 32, textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT, isLoop: true);
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);
            _tailParticleSystem.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            _frameSet.Update(gameTime);
            _graphics.SourceRectangle = _frameSet.GetFrameRect();

            if (_lifetimeCounter < _lifetimeDuration)
            {
                _lifetimeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                if (_frameCounter >= FRAMERATE)
                {
                    _frameCounter = 0;
                    _frameIndex++;

                    if (_frameIndex > FRAME_COUNT - 1)
                    {
                        _frameIndex = 0;
                    }
                }

                var t = _lifetimeCounter / _lifetimeDuration;
                _graphics.Position = Vector2.Lerp(_startPosition, _endPosition, (float)t);
                _graphics.Rotation = MathF.Atan2(_endPosition.Y - _startPosition.Y, _endPosition.X - _startPosition.X);
            }
            else
            {
                if (!IsDestroyed)
                {
                    IsDestroyed = true;
                    _blocker?.Release();
                    if (_targetCombatUnit is not null)
                    {
                        _interaction?.Invoke(_targetCombatUnit);
                    }
                }
            }

            _tailParticleSystem.MoveEmitter(_graphics.Position);
            _tailParticleSystem.Update(gameTime);
        }
    }
}
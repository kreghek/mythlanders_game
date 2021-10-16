﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HealLightObject : IInteractionDelivery
    {
        private const double DURATION_SECONDS = 1.0;
        private const double FRAMERATE = 1f / 8f;

        private const int FRAME_COUNT = 4;

        private readonly AnimationBlocker? _blocker;
        private readonly Vector2 _endPosition;
        private readonly Sprite _graphics;
        private readonly Vector2 _targetPosition;
        private double _counter;
        private double _frameCounter;
        private int _frameIndex;

        public HealLightObject(Vector2 targetPosition, GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker)
        {
            _graphics = new Sprite(contentStorage.GetBulletGraphics());
            _targetPosition = targetPosition;
            _blocker = blocker;
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);
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

                if (_frameCounter >= FRAMERATE)
                {
                    _frameCounter = 0;
                    _frameIndex++;

                    if (_frameIndex > FRAME_COUNT - 1)
                    {
                        _frameIndex = 0;
                    }
                }

                var t = _counter / DURATION_SECONDS;
                _graphics.Position = Vector2.Lerp(_targetPosition, _endPosition, (float)t);
                _graphics.SourceRectangle = new Rectangle(0, 32 * _frameIndex, 64, 32);
            }
            else
            {
                if (!IsDestroyed)
                {
                    IsDestroyed = true;
                    _blocker?.Release();
                }
            }
        }
    }

    internal sealed class BulletGameObject : IInteractionDelivery
    {
        private const double DURATION_SECONDS = 1.0;
        private const double FRAMERATE = 1f / 8f;

        private const int FRAME_COUNT = 4;

        private readonly AnimationBlocker? _blocker;
        private readonly Vector2 _endPosition;
        private readonly Sprite _graphics;
        private readonly Vector2 _startPosition;
        private double _counter;
        private double _frameCounter;
        private int _frameIndex;

        public BulletGameObject(Vector2 startPosition, Vector2 endPosition, GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker)
        {
            _graphics = new Sprite(contentStorage.GetBulletGraphics());
            _startPosition = startPosition;
            _endPosition = endPosition;
            _blocker = blocker;
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);
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

                if (_frameCounter >= FRAMERATE)
                {
                    _frameCounter = 0;
                    _frameIndex++;

                    if (_frameIndex > FRAME_COUNT - 1)
                    {
                        _frameIndex = 0;
                    }
                }

                var t = _counter / DURATION_SECONDS;
                _graphics.Position = Vector2.Lerp(_startPosition, _endPosition, (float)t);
                _graphics.SourceRectangle = new Rectangle(0, 32 * _frameIndex, 64, 32);
            }
            else
            {
                if (!IsDestroyed)
                {
                    IsDestroyed = true;
                    _blocker?.Release();
                }
            }
        }
    }
}
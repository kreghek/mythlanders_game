﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class SymbolObject : IInteractionDelivery
    {
        private const double FRAMERATE = 1f / 8f;
        private const int SYMBOL_SIZE = 128;

        private const int FRAME_COUNT = 18;
        private readonly AnimationBlocker? _blocker;
        private readonly Sprite _graphics;
        private readonly Vector2 _targetPosition;
        private double _counter;
        private double _frameCounter;
        private int _frameIndex;

        public SymbolObject(Vector2 targetPosition, GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker)
        {
            _graphics = new Sprite(contentStorage.GetSymbolSprite());

            _targetPosition = targetPosition;
            _blocker = blocker;
            _graphics.Position = _targetPosition;
            _graphics.SourceRectangle = new Rectangle(0, 0, SYMBOL_SIZE, SYMBOL_SIZE);
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

            _counter += gameTime.ElapsedGameTime.TotalSeconds;
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
                        IsDestroyed = true;
                        _blocker?.Release();
                    }
                }
            }

            _graphics.SourceRectangle = new Rectangle(SYMBOL_SIZE * (_frameIndex % 4), SYMBOL_SIZE * (_frameIndex / 4),
                SYMBOL_SIZE, SYMBOL_SIZE);
        }
    }
}
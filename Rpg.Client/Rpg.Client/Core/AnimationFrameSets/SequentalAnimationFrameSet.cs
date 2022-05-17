using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Core.AnimationFrameSets
{
    internal class SequentalAnimationFrameSet : IAnimationFrameSet
    {
        private double _frameCounter;
        private int _frameListIndex;

        private readonly int _frameWidth;
        private readonly int _frameHeight;
        private readonly int _textureColumns;

        public SequentalAnimationFrameSet(IReadOnlyList<int> frames, int fps, int frameWidth, int frameHeight, int textureColumns)
        {
            _frames = frames;
            _fps = fps;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _textureColumns = textureColumns;
        }

        private readonly IReadOnlyList<int> _frames;

        public bool IsLoop { get; init; }
        public bool IsIdle { get; init; }
        private float _fps { get; }

        public Rectangle GetFrameRect()
        {
            return CalcRect(_frames[_frameListIndex], _textureColumns, _frameWidth, _frameHeight);
        }

        public void Reset()
        {
            _frameCounter = 0;
            _frameListIndex = 0;
        }

        private static Rectangle CalcRect(int frameIndex, int cols, int frameWidth, int frameHeight)
        {
            var col = frameIndex % cols;
            var row = frameIndex / cols;
            return new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (_isEnded)
            {
                return;
            }

            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameCounter > 1 / _fps)
            {
                _frameCounter = 0;
                _frameListIndex++;
                if (_frameListIndex > _frames.Count - 1)
                {
                    if (IsLoop)
                    {
                        _frameListIndex = 0;
                    }
                    else
                    {
                        _frameListIndex = _frames.Count - 1;
                        _isEnded = true;
                        End?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private bool _isEnded;

        public event EventHandler? End;
    }
}
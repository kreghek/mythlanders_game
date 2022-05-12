﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Core
{
    internal record AnimationFrameSet
    {
        private double _frameCounter;
        private int _frameListIndex;

        private readonly int _frameWidth;
        private readonly int _frameHeight;
        private readonly int _textureColumns;

        public AnimationFrameSet(IReadOnlyList<int> frames, int speedMultiplicator, int frameWidth, int frameHeight, int textureColumns)
        {
            _frames = frames;
            SpeedMultiplicator = speedMultiplicator;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _textureColumns = textureColumns;
        }

        private readonly IReadOnlyList<int> _frames;

        public bool IsLoop { get; init; }
        public bool IsIdle { get; init; }
        private float SpeedMultiplicator { get; }

        public double GetDuration()
        {
            return 1 / SpeedMultiplicator * _frames.Count;
        }

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
                
            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds * SpeedMultiplicator;
            if (_frameCounter > 1)
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
                    }
                }
            }
        }

        private bool _isEnded;

        public event EventHandler? End;
    }

    internal static class AnimationFrameSetFactory
    {
        public static AnimationFrameSet CreateSequential(int startFrameIndex, int frameCount, int speedMultiplicator, int frameWidth = 256,
            int frameHeight = 128, int textureColumns = 8, bool isIdle = false, bool isLoop = false)
        {
            var frames = Enumerable.Range(startFrameIndex, frameCount).ToList();
            return new AnimationFrameSet(frames, speedMultiplicator,
                frameWidth, frameHeight, textureColumns)
            {
                IsIdle = isIdle,
                IsLoop = isLoop
            };
        }
    }
}
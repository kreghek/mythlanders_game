﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Client.Core.AnimationFrameSets;

internal class LinearAnimationFrameSet : IAnimationFrameSet
{
    private readonly int _frameHeight;

    private readonly IReadOnlyList<int> _frames;

    private readonly int _frameWidth;
    private readonly int _textureColumns;
    private double _frameCounter;
    private int _frameListIndex;

    private bool _isEnded;

    public LinearAnimationFrameSet(IReadOnlyList<int> frames, float fps, int frameWidth,
        int frameHeight, int textureColumns)
    {
        _frames = frames;
        _fps = fps;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _textureColumns = textureColumns;
    }

    public bool IsLoop { get; init; }

    public IReadOnlyCollection<IAnimationKeyFrame>? KeyFrames { get; init; }
    private float _fps { get; }

    private static Rectangle CalcRect(int frameIndex, int cols, int frameWidth, int frameHeight)
    {
        var col = frameIndex % cols;
        var row = frameIndex / cols;
        return new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
    }

    private void HandleKeyFrames(int currentFrameIndex)
    {
        if (KeyFrames is null)
        {
            return;
        }

        var currentKeyFrame = KeyFrames.SingleOrDefault(x => x.Index == currentFrameIndex);
        if (currentKeyFrame is not null)
        {
            KeyFrameHandled?.Invoke(this, new AnimationKeyFrameEventArgs(currentKeyFrame));
        }
    }

    public bool IsIdle { get; init; }

    public Rectangle GetFrameRect()
    {
        return CalcRect(_frames[_frameListIndex], _textureColumns, _frameWidth, _frameHeight);
    }

    public void Reset()
    {
        _frameCounter = 0;
        _frameListIndex = 0;
        _isEnded = false;
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

            HandleKeyFrames(_frames[_frameListIndex]);
        }
    }

    public event EventHandler? End;
    public event EventHandler<AnimationKeyFrameEventArgs>? KeyFrameHandled;
}
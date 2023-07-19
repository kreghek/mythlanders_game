using System;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.Core.AnimationFrameSets;

internal sealed class SingleFrameSet:  IAnimationFrameSet
{
    private readonly Rectangle _sourceRect;
    private readonly Duration _duration;
    private double _counter;
    private bool _isEnded;

    public SingleFrameSet(Rectangle sourceRect, Duration duration)
    {
        _sourceRect = sourceRect;
        _duration = duration;
    }
    
    public bool IsIdle { get; }
    public Rectangle GetFrameRect() => _sourceRect;

    public void Reset()
    {
        _counter = 0;
    }

    public void Update(GameTime gameTime)
    {
        if (_isEnded)
        {
            return;
        }

        if (_counter < _duration.Seconds)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            _isEnded = true;
            End?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? End;
}
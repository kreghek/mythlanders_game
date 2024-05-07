using System;

using GameClient.Engine;

namespace Client.Engine;

internal sealed class DelayBlocker : IUpdatableAnimationBlocker
{
    private readonly Duration _duration;
    private double _currentCounter;

    public DelayBlocker(Duration duration)
    {
        _duration = duration;
    }

    /// <inheritdoc />
    public AnimationBlockerState State { get; private set; }

    /// <inheritdoc />
    public void Release()
    {
        State = AnimationBlockerState.Released;
        Released?.Invoke(this, EventArgs.Empty);
    }

    public void Update(double elapsedSeconds)
    {
        if (_currentCounter < _duration.Seconds)
        {
            _currentCounter += elapsedSeconds;
        }
        else
        {
            Release();
        }
    }

    /// <inheritdoc />
    public event EventHandler? Released;
}
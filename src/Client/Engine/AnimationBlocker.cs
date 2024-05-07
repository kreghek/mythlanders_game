using System;

namespace Client.Engine;

/// <summary>
/// Base implementation of the animation blocker.
/// </summary>
public sealed class AnimationBlocker : IAnimationBlocker
{
    /// <inheritdoc />
    public AnimationBlockerState State { get; private set; }

    /// <inheritdoc />
    public void Release()
    {
        State = AnimationBlockerState.Released;
        Released?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public event EventHandler? Released;
}

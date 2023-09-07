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

internal interface IUpdatableAnimationBlocker : IAnimationBlocker
{
    void Update(double elapsedSeconds);
}

/// <summary>
/// Blocker of a animation.
/// Used to block UI and combat turn changing until any animation of the current unit is performing.
/// </summary>
public interface IAnimationBlocker
{
    /// <summary>
    /// Current state of the animation.
    /// </summary>
    AnimationBlockerState State { get; }

    /// <summary>
    /// Release blocker. It means the current animation is completed.
    /// </summary>
    void Release();

    /// <summary>
    /// Raise then blocker is released. Used by external code to know the animation complete.
    /// </summary>
    event EventHandler? Released;
}

/// <summary>
/// State of the blocker.
/// </summary>
public enum AnimationBlockerState
{
    /// <summary>
    /// Blocker keeps the current animation.
    /// </summary>
    Performing,

    /// <summary>
    /// The current animation is completed and blocker released.
    /// </summary>
    Released
}
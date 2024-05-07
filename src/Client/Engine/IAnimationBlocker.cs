using System;

namespace Client.Engine;

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

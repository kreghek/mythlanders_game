namespace Client.Engine;

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
namespace GameClient.Engine.Animations;

/// <summary>
/// Event args to pass animation key to event.
/// </summary>
public sealed class AnimationFrameEventArgs : EventArgs
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="keyFrame">Frame info</param>
    public AnimationFrameEventArgs(IAnimationFrameInfo keyFrame)
    {
        KeyFrame = keyFrame;
    }

    /// <summary>
    /// Frame info.
    /// </summary>
    public IAnimationFrameInfo KeyFrame { get; }
}
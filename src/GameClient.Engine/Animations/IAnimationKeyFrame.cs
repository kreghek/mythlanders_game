namespace GameClient.Engine.Animations;

/// <summary>
/// Info of frame info.
/// </summary>
public interface IAnimationFrameInfo: IEquatable<IAnimationFrameInfo>
{
}

/// <summary>
/// The simplest AnimationFrameInfo implementation.
/// </summary>
/// <param name="FrameIndex"> Frame index. </param>
public sealed record AnimationFrameInfo(int FrameIndex): IAnimationFrameInfo
{
    /// <inheritdoc/>
    public bool Equals(IAnimationFrameInfo? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return FrameIndex == (other as AnimationFrameInfo)?.FrameIndex;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return FrameIndex;
    }
}
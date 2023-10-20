using Microsoft.Xna.Framework;

namespace GameClient.Engine.Animations;

/// <summary>
/// Animation to show only single specified frame with specified time.
/// </summary>
/// <remarks>
/// Used with random animation to show simple random frames.
/// </remarks>
public sealed class SingleFrameSet : IAnimationFrameSet
{
    private readonly Duration _duration;
    private readonly Rectangle _sourceRect;
    private double _counter;
    private bool _isEnded;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="fixedSourceRect">Fixed frame rect of animation.</param>
    /// <param name="duration"></param>
    public SingleFrameSet(Rectangle fixedSourceRect, Duration duration)
    {
        _sourceRect = fixedSourceRect;
        _duration = duration;
    }

    /// <inheritdoc />
    public bool IsIdle => false;

    /// <inheritdoc />
    public Rectangle GetFrameRect()
    {
        return _sourceRect;
    }

    /// <inheritdoc />
    public void Reset()
    {
        _counter = 0;
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (_isEnded)
        {
            return;
        }

        if (_counter > 0)
        {
            KeyFrame?.Invoke(this, new AnimationFrameEventArgs(new AnimationFrameInfo(0)));
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

    /// <inheritdoc />
    public event EventHandler? End;

    /// <inheritdoc />
    public event EventHandler<AnimationFrameEventArgs>? KeyFrame;
}
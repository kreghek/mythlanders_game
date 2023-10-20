using Microsoft.Xna.Framework;

namespace GameClient.Engine.Animations;

/// <summary>
/// Animation from multiple animations to play in list sequential way.
/// </summary>
public sealed class CompositeAnimationFrameSet : IAnimationFrameSet
{
    private readonly IReadOnlyList<IAnimationFrameSet> _animationSequence;
    private int _index;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="animationSequence">Animation sequence to play.</param>
    public CompositeAnimationFrameSet(params IAnimationFrameSet[] animationSequence)
    {
        _animationSequence = animationSequence;

        var current = GetCurrentAnimationFrameSet();
        current.End += CurrentAnimation_End;
        current.KeyFrame += CurrentAnimation_KeyFrame;
    }

    private void CurrentAnimation_KeyFrame(object? sender, AnimationFrameEventArgs e)
    {
        KeyFrame?.Invoke(this, e);
    }

    /// <summary>
    /// Make animation to play in infinite cycle.
    /// </summary>
    public bool IsLooping { get; init; }

    private void CurrentAnimation_End(object? sender, EventArgs e)
    {
        var current = GetCurrentAnimationFrameSet();
        current.Reset();
        current.End -= CurrentAnimation_End;

        if (_index < _animationSequence.Count - 1)
        {
            _index++;
            current = GetCurrentAnimationFrameSet();
            current.End += CurrentAnimation_End;
        }
        else
        {
            if (IsLooping)
            {
                _index = 0;
            }
            else
            {
                End?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private IAnimationFrameSet GetCurrentAnimationFrameSet()
    {
        return _animationSequence[_index];
    }

    /// <inheritdoc />
    public bool IsIdle { get; init; }

    /// <inheritdoc />
    public Rectangle GetFrameRect()
    {
        return GetCurrentAnimationFrameSet().GetFrameRect();
    }

    /// <inheritdoc />
    public void Reset()
    {
        _index = 0;

        foreach (var animationFrameSet in _animationSequence)
        {
            animationFrameSet.Reset();
        }
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        var current = GetCurrentAnimationFrameSet();
        current.Update(gameTime);
    }

    /// <inheritdoc />
    public event EventHandler? End;
    
    /// <inheritdoc />
    public event EventHandler<AnimationFrameEventArgs>? KeyFrame;
}
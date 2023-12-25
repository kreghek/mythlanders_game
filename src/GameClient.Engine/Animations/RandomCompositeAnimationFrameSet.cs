using CombatDicesTeam.Dices;

using Microsoft.Xna.Framework;

namespace GameClient.Engine.Animations;

/// <summary>
/// Play random animations from the list.
/// </summary>
public sealed class RandomCompositeAnimationFrameSet : IAnimationFrameSet
{
    private readonly IReadOnlyList<IAnimationFrameSet> _animations;
    private readonly IDice _dice;

    private IAnimationFrameSet _currentAnimation;

    private bool _isEnded;

    private IList<IAnimationFrameSet> _openList;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="animations">List of available animations.</param>
    /// <param name="dice"> Random source to select animations from list. </param>
    public RandomCompositeAnimationFrameSet(IReadOnlyList<IAnimationFrameSet> animations, IDice dice)
    {
        _animations = animations;
        _dice = dice;

        _openList = new List<IAnimationFrameSet>(_animations);

        _currentAnimation = dice.RollFromList(_openList.ToArray());
        _openList.Remove(_currentAnimation);
        _currentAnimation.End += CurrentAnimation_End;

        _currentAnimation.KeyFrame += CurrentAnimation_KeyFrame;
    }

    /// <summary>
    /// Indicate to loop the animation selection cycle.
    /// </summary>
    public bool IsLooping { get; init; }

    private void CurrentAnimation_End(object? sender, EventArgs e)
    {
        _currentAnimation.End -= CurrentAnimation_End;
        _currentAnimation.KeyFrame -= CurrentAnimation_KeyFrame;
        _currentAnimation = _dice.RollFromList(_openList.ToArray());
        _openList.Remove(_currentAnimation);
        _currentAnimation.End += CurrentAnimation_End;
        _currentAnimation.KeyFrame += CurrentAnimation_KeyFrame;
    }

    private void CurrentAnimation_KeyFrame(object? sender, AnimationFrameEventArgs e)
    {
        KeyFrame?.Invoke(this, new AnimationFrameEventArgs(e.KeyFrame));
    }

    /// <inheritdoc />
    public bool IsIdle => false;

    /// <inheritdoc />
    public Rectangle GetFrameRect()
    {
        return _currentAnimation.GetFrameRect();
    }

    /// <inheritdoc />
    public void Reset()
    {
        _currentAnimation.End -= CurrentAnimation_End;
        _openList = new List<IAnimationFrameSet>(_animations);
        _isEnded = false;
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (_isEnded)
        {
            return;
        }

        _currentAnimation.Update(gameTime);

        if (_openList.Any())
        {
            if (!IsLooping)
            {
                End?.Invoke(this, EventArgs.Empty);
            }

            return;
        }

        if (IsLooping)
        {
            Reset();
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
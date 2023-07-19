using System;
using System.Collections.Generic;
using System.Linq;

using Core.Dices;

using Microsoft.Xna.Framework;

namespace Client.Core.AnimationFrameSets;

internal class RandomCompositeAnimationFrameSet : IAnimationFrameSet
{
    private readonly IReadOnlyList<IAnimationFrameSet> _animations;
    private readonly IDice _dice;

    private IAnimationFrameSet _currentAnimation;

    private bool _isEnded;

    private IList<IAnimationFrameSet> _openList;

    public RandomCompositeAnimationFrameSet(IReadOnlyList<IAnimationFrameSet> animations, IDice dice)
    {
        _animations = animations;
        _dice = dice;

        _openList = new List<IAnimationFrameSet>(_animations);

        _currentAnimation = dice.RollFromList(_openList.ToArray());
        _openList.Remove(_currentAnimation);
        _currentAnimation.End += CurrentAnimation_End;
    }

    public bool IsLooping { get; init; }

    private void CurrentAnimation_End(object? sender, EventArgs e)
    {
        _currentAnimation.End -= CurrentAnimation_End;
        _currentAnimation = _dice.RollFromList(_openList.ToArray());
        _openList.Remove(_currentAnimation);
        _currentAnimation.End += CurrentAnimation_End;
    }

    public bool IsIdle { get; init; }

    public Rectangle GetFrameRect()
    {
        return _currentAnimation.GetFrameRect();
    }

    public void Reset()
    {
        _currentAnimation.End -= CurrentAnimation_End;
        _openList = new List<IAnimationFrameSet>(_animations);
        _isEnded = false;
    }

    public void Update(GameTime gameTime)
    {
        if (_isEnded)
        {
            return;
        }

        _currentAnimation.Update(gameTime);

        if (!_openList.Any())
        {
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
    }

    public event EventHandler? End;
}
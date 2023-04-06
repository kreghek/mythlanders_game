using System;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Client.Assets.CombatMovements;

internal sealed class PlayAnimationActorState : IActorVisualizationState
{
    private readonly IAnimationFrameSet _animation;
    private readonly IActorAnimator _animator;
    private readonly Duration _duration;

    private double _counter;

    public PlayAnimationActorState(IActorAnimator animator, IAnimationFrameSet animation,
        Duration? duration = null)
    {
        _animation = animation;
        _animator = animator;
        _duration = duration ?? new Duration(0.25);
        
        _animation.End += Animation_End;
    }

    private void Animation_End(object? sender, EventArgs e)
    {
        IsComplete = true;
    }

    /// <inheritdoc />
    public bool CanBeReplaced => false;
    
    /// <inheritdoc />
    public bool IsComplete { get; private set; }

    /// <inheritdoc />
    public void Cancel()
    {
        if (IsComplete)
        {
        }
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        if (_counter == 0)
        {
            _animator.PlayAnimation(_animation);
        }
        
        _counter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_counter <= _duration.Seconds)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            IsComplete = true;
        }
    }
}
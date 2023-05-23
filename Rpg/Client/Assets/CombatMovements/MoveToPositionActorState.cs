using System;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using MonoGame.Extended.Sprites;

using Rpg.Client.Core;

namespace Client.Assets.CombatMovements;

internal sealed class PlayCustomEffectAnimation : IActorVisualizationState
{
    private readonly IMoveFunction _moveFunction;
    private readonly IAnimationFrameSet _animation;

    /// <inheritdoc />
    public bool CanBeReplaced => true;

    /// <inheritdoc />
    public bool IsComplete { get; private set; }

    public PlayCustomEffectAnimation(IMoveFunction moveFunction, IAnimationFrameSet animation)
    {
        _moveFunction = moveFunction;
        _animation = animation;

        _animation.End += Animation_End;
    }

    private void Animation_End(object? sender, EventArgs e)
    {
        IsComplete = true;
    }

    public void Cancel()
    {
    }

    private bool _animationStarted;

    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        _animation.Update(gameTime);
    }
}

internal sealed class MoveToPositionActorState : IActorVisualizationState
{
    private readonly IAnimationFrameSet _animation;
    private readonly IActorAnimator _animator;
    private readonly Duration _duration;
    private readonly IMoveFunction _moveFunction;

    private double _counter;

    public MoveToPositionActorState(IActorAnimator animator, IMoveFunction moveFunction, IAnimationFrameSet animation,
        Duration? duration = null)
    {
        _animation = animation;
        _animator = animator;
        _moveFunction = moveFunction;
        _duration = duration ?? new Duration(0.25);
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

        if (_counter <= _duration.Seconds)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            var t = _counter / _duration.Seconds;

            var currentPosition = _moveFunction.CalcPosition(t);

            _animator.GraphicRoot.Position = currentPosition;
        }
        else
        {
            IsComplete = true;
            _animator.GraphicRoot.Position = _moveFunction.CalcPosition(1);
        }
    }
}
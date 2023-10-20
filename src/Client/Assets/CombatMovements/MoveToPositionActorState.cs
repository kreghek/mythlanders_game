using System;

using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using GameClient.Engine;
using GameClient.Engine.Animations;
using GameClient.Engine.MoveFunctions;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements;

internal sealed class MoveToPositionActorState : IActorVisualizationState
{
    private readonly IAnimationFrameSet _animation;
    private readonly IActorAnimator _animator;
    private readonly Duration _duration;
    private readonly Func<IMoveFunction> _moveFunctionFactory;

    private double _counter;
    private IMoveFunction? _moveFunction;

    public MoveToPositionActorState(IActorAnimator animator, IMoveFunction moveFunction, IAnimationFrameSet animation,
        Duration? duration = null) : this(animator, () => moveFunction, animation, duration)
    {
        _moveFunction = moveFunction;
    }

    public MoveToPositionActorState(IActorAnimator animator, Func<IMoveFunction> moveFunctionFactory,
        IAnimationFrameSet animation,
        Duration? duration = null)
    {
        _animation = animation;
        _animator = animator;
        _moveFunctionFactory = moveFunctionFactory;
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

        _moveFunction ??= _moveFunctionFactory();

        if (_counter <= _duration.Seconds)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            var t = _counter / _duration.Seconds;

            var currentPosition = _moveFunction.CalcPosition(Math.Min(t, MoveFunctionArgument.Max.Value));

            _animator.GraphicRoot.Position = currentPosition;
        }
        else
        {
            IsComplete = true;
            _animator.GraphicRoot.Position = _moveFunction.CalcPosition(MoveFunctionArgument.Max);
        }
    }
}
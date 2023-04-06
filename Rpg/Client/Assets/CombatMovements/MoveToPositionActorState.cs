using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Client.Assets.CombatMovements;

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

            //var jumpTopPosition = Vector2.UnitY * -24 * (float)Math.Sin((float)_counter / DURATION * Math.PI);

            //var fullPosition = horizontalPosition + jumpTopPosition;

            _animator.GraphicRoot.Position = currentPosition;
        }
        else
        {
            IsComplete = true;
            _animator.GraphicRoot.Position = _moveFunction.CalcPosition(1);
        }
    }
}
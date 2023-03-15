using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat.GameObjects;

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
        if (duration is not null)
        {
            _duration = duration;
        }
        else
        {
            _duration = new Duration(0.25);
        }
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        if (IsComplete)
        {
        }
    }

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
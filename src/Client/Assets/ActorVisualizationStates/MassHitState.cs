using System;

using Client.Core;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

namespace Client.Assets.ActorVisualizationStates;

internal sealed class MassHitState : IActorVisualizationState
{
    private const double DURATION = 1;
    private readonly PredefinedAnimationSid _animationSid;
    private readonly Action _attackInteractions;
    private readonly UnitGraphics _graphics;

    private double _counter;

    private bool _interactionExecuted;

    public MassHitState(UnitGraphics graphics, Action attackInteractions, PredefinedAnimationSid animationSid)
    {
        _graphics = graphics;

        _attackInteractions = attackInteractions;
        _animationSid = animationSid;
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        throw new NotImplementedException();
    }

    public void Update(GameTime gameTime)
    {
        if (_counter == 0)
        {
            _graphics.PlayAnimation(_animationSid);
        }

        _counter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_counter > DURATION)
        {
            IsComplete = true;
        }
        else if (_counter > DURATION / 2)
        {
            if (!_interactionExecuted)
            {
                _interactionExecuted = true;

                _attackInteractions();
            }
        }
    }
}
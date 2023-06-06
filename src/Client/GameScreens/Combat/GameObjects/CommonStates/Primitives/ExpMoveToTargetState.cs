﻿using System;
using System.Diagnostics;

using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects.CommonStates.Primitives;

internal class ExpMoveToTargetState : IActorVisualizationState
{
    private const double DURATION_SECONDS = 0.25;
    private readonly IAnimationFrameSet? _animation;
    private readonly UnitGraphics _graphics;
    private readonly SpriteContainer _graphicsRoot;
    private readonly PredefinedAnimationSid? _predefinedAnimationSid;

    private readonly Vector2 _startPosition;
    private readonly Vector2 _targetPosition;
    private double _counter;

    public ExpMoveToTargetState(UnitGraphics graphics, SpriteContainer graphicsRoot, Vector2 targetPosition,
        PredefinedAnimationSid animationSid)
    {
        _startPosition = graphicsRoot.Position;
        _targetPosition = targetPosition;
        _predefinedAnimationSid = animationSid;
        _graphics = graphics;
        _graphicsRoot = graphicsRoot;
    }

    public ExpMoveToTargetState(UnitGraphics graphics, SpriteContainer graphicsRoot, Vector2 targetPosition,
        IAnimationFrameSet animation)
    {
        _startPosition = graphicsRoot.Position;
        _targetPosition = targetPosition;
        _animation = animation;
        _graphics = graphics;
        _graphicsRoot = graphicsRoot;
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
            if (_predefinedAnimationSid is not null)
            {
                _graphics.PlayAnimation(_predefinedAnimationSid.Value);
            }
            else if (_animation is not null)
            {
                _graphics.PlayAnimation(_animation);
            }
            else
            {
                _graphics.PlayAnimation(PredefinedAnimationSid.Idle);
                Debug.Fail("Any animation must be defined in the constructor.");
            }
        }

        if (_counter <= DURATION_SECONDS)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            var t = _counter / DURATION_SECONDS;

            var horizontalPosition =
                Vector2.Lerp(_startPosition, _targetPosition, (float)Math.Sin(t * Math.PI * 0.5));

            //var jumpTopPosition = Vector2.UnitY * -24 * (float)Math.Sin((float)_counter / DURATION * Math.PI);

            //var fullPosition = horizontalPosition + jumpTopPosition;

            _graphicsRoot.Position = horizontalPosition;
        }
        else
        {
            IsComplete = true;
            _graphicsRoot.Position = _targetPosition;
        }
    }
}
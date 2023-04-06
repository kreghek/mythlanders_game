﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Client.Assets.States.Primitives;

/// <summary>
/// The state starts to play a animation and creates a projectile.
/// </summary>
internal sealed class LaunchAndWaitInteractionDeliveryState : IActorVisualizationState
{
    private readonly IList<IInteractionDelivery> _activeInteractionDeliveryList;

    private readonly IActorAnimator _animator;
    private readonly IAnimationFrameSet _launchAnimation;
    private readonly IReadOnlyCollection<InteractionDeliveryInfo> _imposeItems;
    private readonly IDeliveryFactory _deliveryFactory;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;

    private double _counter;

    public LaunchAndWaitInteractionDeliveryState(IActorAnimator animator,
        IAnimationFrameSet launchAnimation,
        IReadOnlyCollection<InteractionDeliveryInfo> imposeItems,
        IDeliveryFactory deliveryFactory,
        InteractionDeliveryManager interactionDeliveryManager)
    {
        _animator = animator;
        _launchAnimation = launchAnimation;
        _imposeItems = imposeItems;
        _deliveryFactory = deliveryFactory;
        _interactionDeliveryManager = interactionDeliveryManager;

        _activeInteractionDeliveryList = new List<IInteractionDelivery>();
        
        _launchAnimation.End += LaunchAnimation_End;
    }

    private void LaunchAnimation_End(object? sender, EventArgs e)
    {
        LaunchInteractionDelivery();
    }

    private void LaunchInteractionDelivery()
    {
        foreach (var imposeItem in _imposeItems)
        {
            var interactionDelivery = _deliveryFactory.Create(imposeItem.ImposeItem, _animator.GraphicRoot.Position, imposeItem.TargetPosition);

            _interactionDeliveryManager.Register(interactionDelivery);
            _activeInteractionDeliveryList.Add(interactionDelivery);
            interactionDelivery.InteractionPerformed += (sender, _) =>
            {
                if (sender is null)
                {
                    throw new InvalidOperationException();
                }

                _activeInteractionDeliveryList.Remove((IInteractionDelivery)sender);
            };
        }
    }

    /// <inheritdoc />
    public bool CanBeReplaced => false;
    
    /// <inheritdoc />
    public bool IsComplete { get; private set; }

    /// <inheritdoc />
    public void Cancel()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (_counter == 0)
        {
            _animator.PlayAnimation(_launchAnimation);
        }
        
        _counter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_activeInteractionDeliveryList.Any())
        {
            IsComplete = true;
        }
    }
}
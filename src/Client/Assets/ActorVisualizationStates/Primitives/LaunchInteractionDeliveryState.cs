using System;
using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

namespace Client.Assets.ActorVisualizationStates.Primitives;

/// <summary>
/// The state creates a projectile and waits interaction.
/// </summary>
internal sealed class LaunchInteractionDeliveryState : IActorVisualizationState
{
    private readonly IList<IInteractionDelivery> _activeInteractionDeliveryList;

    private readonly IDeliveryFactory _deliveryFactory;
    private readonly IReadOnlyCollection<InteractionDeliveryInfo> _imposeItems;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;

    private bool _interactionDeliveryLaunched;

    public LaunchInteractionDeliveryState(
        IReadOnlyCollection<InteractionDeliveryInfo> imposeItems,
        IDeliveryFactory deliveryFactory,
        InteractionDeliveryManager interactionDeliveryManager)
    {
        _imposeItems = imposeItems;
        _deliveryFactory = deliveryFactory;
        _interactionDeliveryManager = interactionDeliveryManager;

        _activeInteractionDeliveryList = new List<IInteractionDelivery>();
    }

    private static void ImposeEffect(InteractionDeliveryInfo imposeItem)
    {
        foreach (var target in imposeItem.ImposeItem.MaterializedTargets)
        {
            imposeItem.ImposeItem.ImposeDelegate(target);
        }
    }

    private void LaunchInteractionDelivery()
    {
        foreach (var imposeItem in _imposeItems)
        {
            var interactionDelivery = _deliveryFactory.Create(imposeItem.ImposeItem, imposeItem.StartPosition,
                imposeItem.TargetPosition);

            _interactionDeliveryManager.Register(interactionDelivery);
            _activeInteractionDeliveryList.Add(interactionDelivery);
            interactionDelivery.InteractionPerformed += (sender, _) =>
            {
                if (sender is null)
                {
                    throw new InvalidOperationException();
                }

                ImposeEffect(imposeItem);

                _activeInteractionDeliveryList.Remove((IInteractionDelivery)sender);
            };
        }
    }

    /// <inheritdoc />
    public bool CanBeReplaced => false;

    /// <inheritdoc />
    public bool IsComplete => _activeInteractionDeliveryList.Any() && _imposeItems.Any() || !_imposeItems.Any();

    /// <inheritdoc />
    public void Cancel()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (!_interactionDeliveryLaunched)
        {
            LaunchInteractionDelivery();
            _interactionDeliveryLaunched = true;
        }
    }
}
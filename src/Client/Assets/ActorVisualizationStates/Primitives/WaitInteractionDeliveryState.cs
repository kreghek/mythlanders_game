using System;
using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

namespace Client.Assets.ActorVisualizationStates.Primitives;

/// <summary>
/// The state starts to play a animation and creates a projectile.
/// </summary>
internal sealed class WaitInteractionDeliveryState : IActorVisualizationState
{
    private readonly IList<IInteractionDelivery> _activeInteractionDeliveryList;
    private readonly IReadOnlyCollection<IInteractionDelivery> _interactionDeliveryList;
    private readonly IList<IInteractionDelivery> _interactionDeliveryManager;

    private bool _launched;

    public WaitInteractionDeliveryState(
        IReadOnlyCollection<IInteractionDelivery> interactionDeliveryList,
        IList<IInteractionDelivery> interactionDeliveryManager)
    {
        _activeInteractionDeliveryList = new List<IInteractionDelivery>(interactionDeliveryList);

        foreach (var delivery in interactionDeliveryList)
        {
            delivery.InteractionPerformed += (sender, _) =>
            {
                if (sender is null)
                {
                    throw new InvalidOperationException();
                }

                _activeInteractionDeliveryList.Remove((IInteractionDelivery)sender);
            };
        }

        _interactionDeliveryList = interactionDeliveryList;
        _interactionDeliveryManager = interactionDeliveryManager;
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        throw new NotImplementedException();
    }

    public void Update(GameTime gameTime)
    {
        if (!_launched)
        {
            foreach (var item in _interactionDeliveryList)
            {
                _interactionDeliveryManager.Add(item);
            }

            _launched = true;
        }

        // Unit animation is completed
        if (!_activeInteractionDeliveryList.Any())
        {
            // And all interaction delivery animations are completed
            IsComplete = true;
        }
    }
}
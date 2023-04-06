using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.States.Primitives;

internal sealed class InteractionDeliveryManager
{
    private readonly IList<IInteractionDelivery> _interactionDeliveryList = new List<IInteractionDelivery>();

    /// <summary>
    /// Register interaction delivery object in the game.
    /// </summary>
    public void Register(IInteractionDelivery interactionDelivery)
    {
        _interactionDeliveryList.Add(interactionDelivery);
    }

    /// <summary>
    /// Collection of active interaction delivery objects.
    /// </summary>
    public IReadOnlyCollection<IInteractionDelivery> GetActiveSnapshot() => _interactionDeliveryList.ToArray();

    /// <summary>
    /// Remove interaction delivery object from 
    /// </summary>
    public void Unregister(IInteractionDelivery interactionDelivery)
    {
        _interactionDeliveryList.Remove(interactionDelivery);
    }
}
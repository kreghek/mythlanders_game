using System;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements;

internal sealed class InteractionDeliveryFactory : IDeliveryFactory
{
    private readonly Func<Vector2, Vector2, IInteractionDelivery> _createFunc;

    public InteractionDeliveryFactory(Func<Vector2, Vector2, IInteractionDelivery> createFunc)
    {
        _createFunc = createFunc;
    }

    /// <inheritdoc />
    public IInteractionDelivery Create(CombatEffectImposeItem interactionImpose, Vector2 startPoint,
        Vector2 targetPoint)
    {
        return _createFunc(startPoint, targetPoint);
    }
}
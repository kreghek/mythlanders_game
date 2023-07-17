using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Microsoft.Xna.Framework;

namespace Client.Assets.ActorVisualizationStates.Primitives;

internal interface IDeliveryFactory
{
    public IInteractionDelivery Create(CombatEffectImposeItem interactionImpose, Vector2 startPoint,
        Vector2 targetPoint);
}
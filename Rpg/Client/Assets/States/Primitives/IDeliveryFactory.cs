using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Microsoft.Xna.Framework;

namespace Client.Assets.States.Primitives;

internal interface IDeliveryFactory
{
    public IInteractionDelivery Create(CombatEffectImposeItem interactionImpose, Vector2 startPoint, Vector2 targetPoint);
}
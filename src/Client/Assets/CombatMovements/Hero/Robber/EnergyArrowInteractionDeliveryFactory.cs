using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Assets.InteractionDeliveryObjects;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements.Hero.Robber;

internal sealed class EnergyArrowInteractionDeliveryFactory : IDeliveryFactory
{
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    public EnergyArrowInteractionDeliveryFactory(GameObjectContentStorage gameObjectContentStorage)
    {
        _gameObjectContentStorage = gameObjectContentStorage;
    }

    /// <inheritdoc />
    public IInteractionDelivery Create(CombatEffectImposeItem interactionImpose, Vector2 startPoint,
        Vector2 targetPoint)
    {
        return new EnergyArrowProjectile(startPoint, targetPoint, _gameObjectContentStorage.GetBulletGraphics());
    }
}

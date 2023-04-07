using Client.Assets.InteractionDeliveryObjects;
using Client.Assets.States.Primitives;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Microsoft.Xna.Framework;

using Rpg.Client.GameScreens;

namespace Client.Assets.CombatMovements.Hero.Amazon;

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
using System;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Assets.CombatVisualEffects;
using Client.Assets.InteractionDeliveryObjects;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

using GameClient.Engine.CombatVisualEffects;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements.Hero.Robber;

internal sealed class RainingArrowInteractionDeliveryFactory : IDeliveryFactory
{
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly ICombatVisualEffectManager _combatVisualEffectManager;

    public RainingArrowInteractionDeliveryFactory(GameObjectContentStorage gameObjectContentStorage, ICombatVisualEffectManager combatVisualEffectManager)
    {
        _gameObjectContentStorage = gameObjectContentStorage;
        _combatVisualEffectManager = combatVisualEffectManager;
    }

    /// <inheritdoc />
    public IInteractionDelivery Create(CombatEffectImposeItem interactionImpose, Vector2 startPoint,
        Vector2 targetPoint)
    {
        var arrow = new EnergyArrowProjectile(startPoint, targetPoint, _gameObjectContentStorage.GetBulletGraphics(),
            0.25f + GetRandomOffset(startPoint, 1));
        
        arrow.InteractionPerformed += (_, _) =>
        {
            var blast = new EnergyBlastVisualEffect(
                targetPoint,
                _gameObjectContentStorage.GetBulletGraphics(),
                _gameObjectContentStorage.GetParticlesTexture());
            
            _combatVisualEffectManager.AddEffect(blast);
        };

        return arrow;
    }

    private static double GetRandomOffset(Vector2 position, double baseValue)
    {
        var v = (position.X * 13 + position.Y * 7) * 0.1;
        var intPart = Math.Floor(v);
        var rndOffset = v - intPart;
        return baseValue * rndOffset;  
    }
}
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
            0.5f + GetRandomOffset(startPoint, 0.25));
        
        arrow.InteractionPerformed += (_, _) =>
        {
            var blast = new EnergyBlastVisualEffect(
                targetPoint,
                _gameObjectContentStorage.GetBulletGraphics(),
                _gameObjectContentStorage.GetParticlesTexture());
            
            _combatVisualEffectManager.AddEffect(blast);
        };

        var sequentialProjectile = new SequentialProjectile(new IInteractionDelivery[] { arrow/*, blast*/ });

        return sequentialProjectile;
    }

    private static double GetRandomOffset(Vector2 position, double baseValue)
    {
        var v = Math.Pow(position.X, position.Y);
        var intPart = (int)v;
        return baseValue * (v - intPart);  
    }
}
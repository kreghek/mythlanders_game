using System;
using System.Collections.Generic;

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
    private readonly ICombatVisualEffectManager _combatVisualEffectManager;
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    public RainingArrowInteractionDeliveryFactory(GameObjectContentStorage gameObjectContentStorage,
        ICombatVisualEffectManager combatVisualEffectManager)
    {
        _gameObjectContentStorage = gameObjectContentStorage;
        _combatVisualEffectManager = combatVisualEffectManager;
    }

    private static double GetRandomOffset(Vector2 position, double baseValue)
    {
        var v = (position.X * 13 + position.Y * 7) * 0.1;
        var intPart = Math.Floor(v);
        var rndOffset = v - intPart;
        return baseValue * rndOffset;
    }

    /// <inheritdoc />
    public IInteractionDelivery Create(CombatEffectImposeItem interactionImpose, Vector2 startPoint,
        Vector2 targetPoint)
    {
        var rnd = GetRandomOffset(startPoint, 1);

        var arrow = new EnergyArrowProjectile(startPoint, targetPoint, _gameObjectContentStorage.GetBulletGraphics(),
            0.25f + rnd);


        arrow.InteractionPerformed += (s, _) =>
        {
            var blast = new EnergyBlastVisualEffect(
                targetPoint,
                _gameObjectContentStorage.GetBulletGraphics(),
                _gameObjectContentStorage.GetParticlesTexture());

            _combatVisualEffectManager.AddEffect(blast);

            var sfx = _gameObjectContentStorage
                        .GetSkillUsageSound(GameObjectSoundType.ImpulseArrowBlasts)
                        .CreateInstance();
            sfx.Pitch = (float)rnd * 2 - 1;
            sfx.Play();
        };

        return arrow;
    }
}
﻿using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;

namespace GameAssets.Combats.CombatMovementEffects;

public sealed class SwapPositionEffectInstance : EffectInstanceBase<SwapPositionEffect>
{
    public SwapPositionEffectInstance(SwapPositionEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        var actorSide = GetTargetSide(context.Actor, context.Field);

        var actorCoords = actorSide.GetCombatantCoords(context.Actor);

        var targetSide = GetTargetSide(target, context.Field);

        var targetCoords = targetSide.GetCombatantCoords(target);

        context.NotifySwapFieldPosition(context.Actor, actorCoords, actorSide, targetCoords, targetSide,
            new PositionChangeReason());
    }

    private static CombatFieldSide GetTargetSide(ICombatant target, CombatField field)
    {
        try
        {
            var _ = field.HeroSide.GetCombatantCoords(target);
            return field.HeroSide;
        }
        catch (ArgumentException)
        {
            var _ = field.MonsterSide.GetCombatantCoords(target);
            return field.MonsterSide;
        }
    }
}
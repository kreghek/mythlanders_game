using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;

namespace GameAssets.Combats.CombatMovementEffects;

public sealed class PushToPositionEffectInstance : EffectInstanceBase<PushToPositionEffect>
{
    public PushToPositionEffectInstance(PushToPositionEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        if (target.IsDead)
        {
            return;
        }

        var targetSide = GetTargetSide(target, context.Field);

        var currentCoords = targetSide.GetCombatantCoords(target);

        var targetCoords = BaseEffect.Direction switch
        {
            ChangePositionEffectDirection.ToVanguard => currentCoords with
            {
                ColumentIndex = 0
            },
            ChangePositionEffectDirection.ToRearguard => currentCoords with
            {
                ColumentIndex = 1
            },
            _ => throw new InvalidOperationException($"Unknown direction {BaseEffect.Direction}")
        };

        context.NotifySwapFieldPosition(target, currentCoords, targetSide, targetCoords, targetSide,
            new PositionChangeReason());
    }

    private static CombatFieldSide GetTargetSide(ICombatant target, CombatField field)
    {
        if (IsCombatantInSide(target, field.HeroSide))
        {
            return field.HeroSide;
        }

        return field.MonsterSide;
    }

    private static bool IsCombatantInSide(ICombatant target, CombatFieldSide side)
    {
        return side.GetAllCombatants().Any(x => x == target);
    }
}
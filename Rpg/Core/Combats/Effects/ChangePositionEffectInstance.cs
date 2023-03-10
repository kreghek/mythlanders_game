namespace Core.Combats.Effects;

public sealed class ChangePositionEffectInstance : EffectInstanceBase<ChangePositionEffect>
{
    public ChangePositionEffectInstance(ChangePositionEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
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
            _ => throw new ArgumentOutOfRangeException()
        };

        var targetCombatant = targetSide[targetCoords].Combatant;

        context.NotifyCombatantMoved(target, targetCoords);

        // Swap with combatant in the target coords
        if (targetCombatant is not null)
        {
            context.NotifyCombatantMoved(targetCombatant, currentCoords);
        }
    }

    private static CombatFieldSide GetTargetSide(Combatant target, CombatField field)
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
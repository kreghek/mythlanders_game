namespace Core.Combats.Effects;

public sealed class PushToPositionEffectInstance : EffectInstanceBase<PushToPositionEffect>
{
    public PushToPositionEffectInstance(PushToPositionEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IStatusCombatContext context)
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

        context.NotifySwapFieldPosition(target, currentCoords, targetSide, targetCoords, targetSide);
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
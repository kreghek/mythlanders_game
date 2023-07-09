namespace Core.Combats.Effects;

public sealed class SwapPositionEffectInstance : EffectInstanceBase<SwapPositionEffect>
{
    public SwapPositionEffectInstance(SwapPositionEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IStatusCombatContext context)
    {
        var actorSide = GetTargetSide(context.Actor, context.Field);

        var actorCoords = actorSide.GetCombatantCoords(context.Actor);

        var targetSide = GetTargetSide(target, context.Field);

        var targetCoords = targetSide.GetCombatantCoords(target);

        context.NotifySwapFieldPosition(context.Actor, actorCoords, actorSide, targetCoords, targetSide);
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
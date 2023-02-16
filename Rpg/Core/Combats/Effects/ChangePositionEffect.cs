namespace Core.Combats.Effects;

public sealed class ChangePositionEffect : IEffect
{
    public ChangePositionEffect(ITargetSelector selector,
        ChangePositionEffectDirection direction)
    {
        Direction = direction;

        Selector = selector;
    }

    public ChangePositionEffectDirection Direction { get; }

    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ChangePositionEffectInstance(this);
    }
}

public sealed class ChangePositionEffectInstance : EffectInstanceBase<ChangePositionEffect>
{
    public ChangePositionEffectInstance(ChangePositionEffect baseEffect): base(baseEffect)
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

        targetSide[targetCoords].Combatant = target;
        targetSide[currentCoords].Combatant = targetCombatant;
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

public enum ChangePositionEffectDirection
{
    ToVanguard,
    ToRearguard
}
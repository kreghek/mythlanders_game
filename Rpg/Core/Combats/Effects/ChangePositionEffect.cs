namespace Core.Combats.Effects;

public sealed class ChangePositionEffect: IEffect
{
    public ChangePositionEffectDirection Direction { get; }

    public ChangePositionEffect(ITargetSelector selector, IEffectImposer imposer, ChangePositionEffectDirection direction)
    {
        Direction = direction;

        Selector = selector;
        Imposer = imposer;
    }
    
    public ITargetSelector Selector { get; }
    public IEffectImposer Imposer { get; }
    public void Influence(Combatant target, IEffectCombatContext context)
    {
        var currentCoords = context.Field.HeroSide.GetCombatantCoords(target);

        var targetCoords = Direction switch
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

        var targetCombatant = context.Field.HeroSide[targetCoords].Combatant;

        context.Field.HeroSide[targetCoords].Combatant = target;
        context.Field.HeroSide[currentCoords].Combatant = targetCombatant;
    }
}

public enum ChangePositionEffectDirection
{
    ToVanguard,
    ToRearguard
}
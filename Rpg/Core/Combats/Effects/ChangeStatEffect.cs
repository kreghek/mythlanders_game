namespace Core.Combats.Effects;

public sealed class ChangeStatEffect : IEffect
{
    private readonly ICombatantEffectLifetimeFactory _combatantEffectLifetimeFactory;

    public ChangeStatEffect(ITargetSelector selector, UnitStatType statType, int value,
        ICombatantEffectLifetimeFactory combatantEffectLifetimeFactory)
    {
        _combatantEffectLifetimeFactory = combatantEffectLifetimeFactory;
        TargetStatType = statType;
        Selector = selector;
        Value = value;
    }

    public UnitStatType TargetStatType { get; }
    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new ChangeStatEffectInstance(this, _combatantEffectLifetimeFactory.Create());
    }
}
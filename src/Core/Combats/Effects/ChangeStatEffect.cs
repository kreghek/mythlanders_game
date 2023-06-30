namespace Core.Combats.Effects;

public sealed class ChangeStatEffect : IEffect
{
    private readonly ICombatantEffectSid _combatantEffectSid;
    private readonly ICombatantEffectLifetimeFactory _combatantEffectLifetimeFactory;

    public ChangeStatEffect(ICombatantEffectSid combatantEffectSid, ITargetSelector selector, UnitStatType statType, int value,
        ICombatantEffectLifetimeFactory combatantEffectLifetimeFactory)
    {
        _combatantEffectSid = combatantEffectSid;
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
        return new ChangeStatEffectInstance(_combatantEffectSid, this, _combatantEffectLifetimeFactory.Create());
    }
}
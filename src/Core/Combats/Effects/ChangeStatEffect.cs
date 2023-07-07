namespace Core.Combats.Effects;

public sealed class ChangeStatEffect : IEffect
{
    private readonly ICombatantEffectLifetimeFactory _combatantEffectLifetimeFactory;
    private readonly ICombatantStatusSid _combatantEffectSid;

    public ChangeStatEffect(ICombatantStatusSid combatantEffectSid, ITargetSelector selector, ICombatantStatType statType,
        int value,
        ICombatantEffectLifetimeFactory combatantEffectLifetimeFactory)
    {
        _combatantEffectSid = combatantEffectSid;
        _combatantEffectLifetimeFactory = combatantEffectLifetimeFactory;
        TargetStatType = statType;
        Selector = selector;
        Value = value;
    }

    public ICombatantStatType TargetStatType { get; }
    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new ChangeStatEffectInstance(_combatantEffectSid, this, _combatantEffectLifetimeFactory.Create());
    }
}
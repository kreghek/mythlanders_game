using Core.Combats.CombatantEffects;

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

public sealed class AddCombatantEffectEffect : IEffect
{
    private readonly ICombatantEffectFactory _combatantEffectFactory;


    public AddCombatantEffectEffect(ITargetSelector targetSelector, ICombatantEffectFactory combatantEffectFactory)
    {
        _combatantEffectFactory = combatantEffectFactory;

        Selector = targetSelector;
    }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();
    public ITargetSelector Selector { get; }
    public IEffectInstance CreateInstance()
    {
        return new AddCombatantEffectEffectInstance(this, _combatantEffectFactory);
    }
}

public sealed class AddCombatantEffectEffectInstance: EffectInstanceBase<AddCombatantEffectEffect>
{
    private readonly ICombatantEffectFactory _combatantEffectFactory;

    public AddCombatantEffectEffectInstance(AddCombatantEffectEffect baseEffect,
        ICombatantEffectFactory combatantEffectFactory) : base(baseEffect)
    {
        _combatantEffectFactory = combatantEffectFactory;
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        target.AddEffect(_combatantEffectFactory.Create());
    }
}
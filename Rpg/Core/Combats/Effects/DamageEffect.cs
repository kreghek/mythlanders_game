namespace Core.Combats.Effects;

public sealed class DamageEffect : IEffect
{
    public DamageEffect(ITargetSelector selector, DamageType damageType, Range<int> damage)
    {
        Selector = selector;
        DamageType = damageType;
        Damage = damage;
    }

    public Range<int> Damage { get; }
    public DamageType DamageType { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new DamageEffectInstance(this);
    }
}

public sealed class PeriodicEffect : IEffect
{
    public PeriodicEffect(ITargetSelector selector, IEffect baseEffect, ICombatantEffectLifetime lifetime)
    {
        Selector = selector;
        BaseEffect = baseEffect;
        Lifetime = lifetime;
    }

    public ITargetSelector Selector { get; }
    public IEffect BaseEffect { get; }
    public ICombatantEffectLifetime Lifetime { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new PeriodicEffectInstance(this);
    }
}

public sealed class PeriodicEffectInstance : EffectInstanceBase<PeriodicEffect>
{
    public PeriodicEffectInstance(PeriodicEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        throw new NotImplementedException();
    }
}

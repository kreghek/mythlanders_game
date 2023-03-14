namespace Core.Combats.Effects;

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

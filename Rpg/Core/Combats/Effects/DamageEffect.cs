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

namespace Core.Combats;

public sealed class DamageEffect: IEffect
{
    private readonly ITargetSelector _selector;
    private readonly IEffectLifetime _lifetime;

    public DamageEffect(ITargetSelector selector, IEffectLifetime lifetime)
    {
        _selector = selector;
        _lifetime = lifetime;
    }

    public ITargetSelector Selector => _selector;
    public IEffectLifetime Lifetime => _lifetime;
}
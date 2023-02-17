namespace Core.Combats.Effects;

public sealed class ChangeStatEffect : IEffect
{
    public ChangeStatEffect(ITargetSelector selector, UnitStatType statType, int value,
        Type lifetimeType)
    {
        TargetStatType = statType;
        Selector = selector;
        Value = value;
        LifetimeType = lifetimeType;
    }

    public Type LifetimeType { get; }
    public UnitStatType TargetStatType { get; }
    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ChangeStatEffectInstance(this);
    }
}
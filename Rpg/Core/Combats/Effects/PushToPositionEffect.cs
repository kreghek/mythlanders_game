namespace Core.Combats.Effects;

public sealed class PushToPositionEffect : IEffect
{
    public PushToPositionEffect(ITargetSelector selector,
        ChangePositionEffectDirection direction)
    {
        Direction = direction;

        Selector = selector;
    }

    public ChangePositionEffectDirection Direction { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new PushToPositionEffectInstance(this);
    }
}

public sealed class AdjustPositionEffect : IEffect
{
    public AdjustPositionEffect(ITargetSelector selector)
    {
        Selector = selector;
    }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new AdjustPositionEffectInstance(this);
    }
}
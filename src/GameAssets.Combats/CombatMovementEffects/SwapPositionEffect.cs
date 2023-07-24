namespace Core.Combats.Effects;

public sealed class SwapPositionEffect : IEffect
{
    public SwapPositionEffect(ITargetSelector selector)
    {
        Selector = selector;
    }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new SwapPositionEffectInstance(this);
    }
}
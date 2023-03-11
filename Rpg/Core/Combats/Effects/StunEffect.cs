namespace Core.Combats.Effects;

public class StunEffect : IEffect
{
    public StunEffect(ITargetSelector selector, Range<int> heal)
    {
        Selector = selector;
        Heal = heal;
    }

    public ITargetSelector Selector { get; }
    public Range<int> Heal { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new StunEffectInstance(this);
    }
}
namespace Core.Combats.Effects;

public class HealEffect : IEffect
{
    public HealEffect(ITargetSelector selector, Range<int> heal)
    {
        Selector = selector;
        Heal = heal;
    }

    public Range<int> Heal { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new HealEffectInstance(this);
    }
}
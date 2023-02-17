using Core.Combats;

namespace Core.Combats.Effects;

public sealed class ChangeCurrentStatEffect : IEffect
{
    public ChangeCurrentStatEffect(ITargetSelector selector, UnitStatType statType,
        Range<int> statValue)
    {
        TargetStatType = statType;
        Selector = selector;
        StatValue = statValue;
    }

    public Range<int> StatValue { get; }
    public UnitStatType TargetStatType { get; }
    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ChangeCurrentStatEffectInstance(this);
    }
}
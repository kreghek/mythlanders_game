namespace Core.Combats;

public interface IEffect
{
    ITargetSelector Selector { get; }

    IEffectInstance CreateInstance();

    IReadOnlyCollection<IEffectCondition> ImposeConditions { get; }
}
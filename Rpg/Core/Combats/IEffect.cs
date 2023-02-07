namespace Core.Combats;

public interface IEffect
{
    ITargetSelector Selector { get; }
    
    IEffectLifetime Lifetime { get; }
}
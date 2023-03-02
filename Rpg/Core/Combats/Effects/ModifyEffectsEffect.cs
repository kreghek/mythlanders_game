namespace Core.Combats.Effects;

public sealed class ModifyEffectsEffect : IEffect
{
    public ModifyEffectsEffect(ITargetSelector selector, int value)
    {
        Selector = selector;
        Value = value;
    }

    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ModifyEffectsEffectInstance(this);
    }
}
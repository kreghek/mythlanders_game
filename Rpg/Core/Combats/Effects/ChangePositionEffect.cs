namespace Core.Combats.Effects;

public sealed class ChangePositionEffect : IEffect
{
    public ChangePositionEffect(ITargetSelector selector,
        ChangePositionEffectDirection direction)
    {
        Direction = direction;

        Selector = selector;
    }

    public ChangePositionEffectDirection Direction { get; }

    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new ChangePositionEffectInstance(this);
    }
}
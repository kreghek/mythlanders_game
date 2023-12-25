using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;

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
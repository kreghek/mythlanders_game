using CombatDicesTeam.Combats;
using CombatDicesTeam.GenericRanges;

namespace Core.Combats.Effects;

public sealed class LifeDrawEffect : IEffect
{
    public LifeDrawEffect(ITargetSelector selector, GenericRange<int> damage)
    {
        Selector = selector;
        Damage = damage;
    }

    public GenericRange<int> Damage { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new LifeDrawEffectInstance(this);
    }
}
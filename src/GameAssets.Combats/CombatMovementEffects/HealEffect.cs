using CombatDicesTeam.Combats;
using CombatDicesTeam.GenericRanges;

namespace Core.Combats.Effects;

public class HealEffect : IEffect
{
    public HealEffect(ITargetSelector selector, GenericRange<int> heal)
    {
        Selector = selector;
        Heal = heal;
    }

    public GenericRange<int> Heal { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new HealEffectInstance(this);
    }
}
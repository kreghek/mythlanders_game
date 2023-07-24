using GameAssets.Combats.CombatMovementEffects;

namespace Core.Combats.Effects;

public sealed class MarkEffect : IEffect
{
    private readonly ICombatantStatusLifetimeFactory _combatantEffectLifetimeFactory;

    public MarkEffect(ITargetSelector selector, ICombatantStatusLifetimeFactory combatantEffectLifetimeFactory)
    {
        _combatantEffectLifetimeFactory = combatantEffectLifetimeFactory;
        Selector = selector;
    }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();
    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new MarkEffectInstance(this, _combatantEffectLifetimeFactory.Create());
    }
}
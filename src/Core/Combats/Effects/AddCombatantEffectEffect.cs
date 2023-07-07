using Core.Combats.CombatantStatus;

namespace Core.Combats.Effects;

public sealed class AddCombatantEffectEffect : IEffect
{
    private readonly ICombatantStatusFactory _combatantEffectFactory;


    public AddCombatantEffectEffect(ITargetSelector targetSelector, ICombatantStatusFactory combatantEffectFactory)
    {
        _combatantEffectFactory = combatantEffectFactory;

        Selector = targetSelector;
    }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();
    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new AddCombatantEffectEffectInstance(this, _combatantEffectFactory);
    }
}
using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class AddCombatantEffectEffect : IEffect
{
    private readonly ICombatantEffectFactory _combatantEffectFactory;


    public AddCombatantEffectEffect(ITargetSelector targetSelector, ICombatantEffectFactory combatantEffectFactory)
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
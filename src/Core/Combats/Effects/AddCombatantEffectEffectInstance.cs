using Core.Combats.CombatantStatuses;

namespace Core.Combats.Effects;

public sealed class AddCombatantEffectEffectInstance : EffectInstanceBase<AddCombatantEffectEffect>
{
    private readonly ICombatantStatusFactory _combatantEffectFactory;

    public AddCombatantEffectEffectInstance(AddCombatantEffectEffect baseEffect,
        ICombatantStatusFactory combatantEffectFactory) : base(baseEffect)
    {
        _combatantEffectFactory = combatantEffectFactory;
    }

    public override void Influence(Combatant target, IStatusCombatContext context)
    {
        context.StatusImposedContext.Combat.ImposeCombatantEffect(target, _combatantEffectFactory.Create());
    }
}
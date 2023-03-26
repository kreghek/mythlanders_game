using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class AddCombatantEffectEffectInstance : EffectInstanceBase<AddCombatantEffectEffect>
{
    private readonly ICombatantEffectFactory _combatantEffectFactory;

    public AddCombatantEffectEffectInstance(AddCombatantEffectEffect baseEffect,
        ICombatantEffectFactory combatantEffectFactory) : base(baseEffect)
    {
        _combatantEffectFactory = combatantEffectFactory;
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        target.AddEffect(_combatantEffectFactory.Create(), context.EffectImposedContext);
    }
}
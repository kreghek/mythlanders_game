using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantStatuses;

namespace Core.Combats.Effects;

public sealed class ModifyEffectsEffectInstance : EffectInstanceBase<ModifyEffectsEffect>
{
    private readonly ICombatantStatusSid _effectSid;

    public ModifyEffectsEffectInstance(ICombatantStatusSid effectSid, ModifyEffectsEffect baseEffect) : base(baseEffect)
    {
        _effectSid = effectSid;
    }

    public override void Influence(Combatant target, IStatusCombatContext context)
    {
        var combatantEffect = new ModifyEffectsCombatantStatus(
            _effectSid,
            new MultipleCombatantTurnEffectLifetime(1),
            BaseEffect.Value);

        context.StatusImposedContext.Combat.ImposeCombatantEffect(target, combatantEffect);
    }
}
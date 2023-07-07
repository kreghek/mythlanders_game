using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ModifyEffectsEffectInstance : EffectInstanceBase<ModifyEffectsEffect>
{
    private readonly ICombatantStatusSid _effectSid;

    public ModifyEffectsEffectInstance(ICombatantStatusSid effectSid, ModifyEffectsEffect baseEffect) : base(baseEffect)
    {
        _effectSid = effectSid;
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var combatantEffect = new ModifyEffectsCombatantEffect(
            _effectSid,
            new MultipleCombatantTurnEffectLifetime(1),
            BaseEffect.Value);

        context.EffectImposedContext.Combat.ImposeCombatantEffect(target, combatantEffect);
    }
}
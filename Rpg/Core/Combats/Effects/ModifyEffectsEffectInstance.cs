using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ModifyEffectsEffectInstance : EffectInstanceBase<ModifyEffectsEffect>
{
    public ModifyEffectsEffectInstance(ModifyEffectsEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        target.AddEffect(new ModifyEffectsCombatantEffect(new MultipleCombatantTurnEffectLifetime(2),
            BaseEffect.Value), context.EffectImposedContext);
    }
}
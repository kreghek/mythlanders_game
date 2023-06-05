using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ChangeStatEffectInstance : EffectInstanceBase<ChangeStatEffect>
{
    public ChangeStatEffectInstance(ChangeStatEffect baseEffect, ICombatantEffectLifetime combatantEffectLifetime) :
        base(baseEffect)
    {
        Lifetime = combatantEffectLifetime;
    }

    public ICombatantEffectLifetime Lifetime { get; }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var combatantEffect =
            new ChangeStatCombatantEffect(Lifetime, BaseEffect.TargetStatType, BaseEffect.Value);
        target.AddEffect(combatantEffect, context.EffectImposedContext);
    }
}
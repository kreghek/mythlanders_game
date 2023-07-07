using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ChangeStatEffectInstance : EffectInstanceBase<ChangeStatEffect>
{
    private readonly ICombatantStatusSid _combatantEffectSid;

    public ChangeStatEffectInstance(ICombatantStatusSid combatantEffectSid, ChangeStatEffect baseEffect,
        ICombatantStatusLifetime combatantEffectLifetime) :
        base(baseEffect)
    {
        _combatantEffectSid = combatantEffectSid;
        Lifetime = combatantEffectLifetime;
    }

    public ICombatantStatusLifetime Lifetime { get; }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var combatantEffect =
            new ChangeStatCombatantEffect(_combatantEffectSid, Lifetime, BaseEffect.TargetStatType, BaseEffect.Value);

        context.EffectLifetimeImposedContext.Combat.ImposeCombatantEffect(target, combatantEffect);
    }
}
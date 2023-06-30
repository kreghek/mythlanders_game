using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class ChangeStatEffectInstance : EffectInstanceBase<ChangeStatEffect>
{
    private readonly ICombatantEffectSid _combatantEffectSid;

    public ChangeStatEffectInstance(ICombatantEffectSid combatantEffectSid,  ChangeStatEffect baseEffect, ICombatantEffectLifetime combatantEffectLifetime) :
        base(baseEffect)
    {
        _combatantEffectSid = combatantEffectSid;
        Lifetime = combatantEffectLifetime;
    }

    public ICombatantEffectLifetime Lifetime { get; }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var combatantEffect =
            new ChangeStatCombatantEffect(_combatantEffectSid, Lifetime, BaseEffect.TargetStatType, BaseEffect.Value);
        target.AddEffect(combatantEffect, context.EffectImposedContext, context.EffectLifetimeImposedContext);
    }
}
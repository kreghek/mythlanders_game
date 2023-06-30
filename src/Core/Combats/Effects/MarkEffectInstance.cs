using Core.Combats.CombatantEffects;

namespace Core.Combats.Effects;

public sealed class MarkEffectInstance : EffectInstanceBase<MarkEffect>
{
    private readonly ICombatantEffectLifetime _lifetime;

    public MarkEffectInstance(MarkEffect baseEffect, ICombatantEffectLifetime lifetime) : base(baseEffect)
    {
        _lifetime = lifetime;
    }

    public override void Influence(Combatant target, IEffectCombatContext context)
    {
        var markEffectSid = CombatantEffectSids.Mark;
        
        target.AddEffect(new MarkCombatantEffect(markEffectSid, _lifetime),
            context.EffectImposedContext, 
            context.EffectLifetimeImposedContext);
    }
}
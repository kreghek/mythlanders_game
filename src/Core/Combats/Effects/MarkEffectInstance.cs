using Core.Combats.CombatantStatuses;

namespace Core.Combats.Effects;

public sealed class MarkEffectInstance : EffectInstanceBase<MarkEffect>
{
    private readonly ICombatantStatusLifetime _lifetime;

    public MarkEffectInstance(MarkEffect baseEffect, ICombatantStatusLifetime lifetime) : base(baseEffect)
    {
        _lifetime = lifetime;
    }

    public override void Influence(Combatant target, IStatusCombatContext context)
    {
        var markEffectSid = CombatantStatusSids.Mark;

        context.StatusImposedContext.Combat.ImposeCombatantEffect(target,
            new MarkCombatantStatus(markEffectSid, _lifetime));
    }
}
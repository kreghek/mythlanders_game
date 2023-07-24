using Core.Combats;
using Core.Combats.CombatantStatuses;
using Core.Combats.Effects;

namespace GameAssets.Combats.CombatMovementEffects;

public sealed class MarkEffectInstance : EffectInstanceBase<MarkEffect>
{
    private readonly ICombatantStatusLifetime _lifetime;

    public MarkEffectInstance(MarkEffect baseEffect, ICombatantStatusLifetime lifetime) : base(baseEffect)
    {
        _lifetime = lifetime;
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        var markEffectSid = CombatantStatusSids.Mark;
        
        context.StatusImposedContext.ImposeCombatantStatus(target,
            new DelegateCombatStatusFactory(() => new MarkCombatantStatus(markEffectSid, _lifetime)));
    }
}
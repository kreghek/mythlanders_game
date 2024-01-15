using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;

using GameAssets.Combats.CombatantStatuses;

namespace GameAssets.Combats.CombatMovementEffects;

public sealed class MarkEffectInstance : EffectInstanceBase<MarkEffect>
{
    private readonly ICombatantStatusLifetime _lifetime;

    public MarkEffectInstance(MarkEffect baseEffect, ICombatantStatusLifetime lifetime) : base(baseEffect)
    {
        _lifetime = lifetime;
    }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        var markEffectSid = CombatantStatusSids.Mark;

        context.StatusImposedContext.ImposeCombatantStatus(target,
            new CombatMovementCombatantStatusSource(context.Actor),
            new CombatStatusFactory(source => new MarkCombatantStatus(markEffectSid, _lifetime, source)));
    }
}
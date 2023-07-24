using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats.CombatantStatuses;

public sealed class ImpulseGeneratorCombatantStatus : CombatantStatusBase
{
    private const int GENERATED_LIMIT = 2;
    private const int DAMAGE_BONUS = 1;
    private const int SURGE_DAMAGE = 3;

    private readonly ICombatantStatusSid _generatedSid;
    private Combatant? _effectOwner;

    public ImpulseGeneratorCombatantStatus(ICombatantStatusSid sid, ICombatantStatusSid generatedSid,
        ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
        _generatedSid = generatedSid;
    }

    public override void Impose(Combatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        _effectOwner = combatant;

        combatantEffectImposeContext.Combat.CombatantHasChangePosition += Combat_CombatantHasChangePosition;
    }

    private IReadOnlyCollection<ICombatantStatus> CollectImpulseEffects(Combatant targetCombatant)
    {
        return targetCombatant.Statuses.Where(x => x.Sid == _generatedSid).ToArray();
    }

    private void Combat_CombatantHasChangePosition(object? sender, CombatantHasChangedPositionEventArgs e)
    {
        var targetCombatant = e.Combatant;

        if (targetCombatant != _effectOwner)
        {
            // Event handler will raise for every combatants because it subscribed on whole combat.
            // So we must check effect bearer.
            return;
        }

        var targetCombat = (CombatEngineBase)sender!;

        var currentGeneratedCount = targetCombatant.Statuses.Count(x => x.Sid == _generatedSid);

        if (currentGeneratedCount < GENERATED_LIMIT)
        {
            // Impulse effect lives until combatant makes an attacking movement.
            GainImpulseUnit(targetCombatant, targetCombat);
        }
        else
        {
            ImpulseSurge(targetCombatant, targetCombat);
        }
    }

    private void GainImpulseUnit(Combatant targetCombatant, CombatEngineBase targetCombat)
    {
        // effect life ends on attack.
        var lifetime = new UntilCombatantEffectMeetPredicatesLifetime(new[]
        {
            new IsAttackCombatMovePredicate()
        });

        var impulseCombatantEffect = new ModifyEffectsCombatantStatus(_generatedSid,
            lifetime, DAMAGE_BONUS);

        targetCombat.ImposeCombatantEffect(targetCombatant, impulseCombatantEffect);
    }

    private void ImpulseSurge(Combatant targetCombatant, CombatEngineBase targetCombat)
    {
        var impulseEffects = CollectImpulseEffects(targetCombatant);
        foreach (var combatantEffect in impulseEffects)
        {
            targetCombat.DispelCombatantEffect(targetCombatant, combatantEffect);
        }

        targetCombat.HandleCombatantDamagedToStat(targetCombatant, CombatantStatTypes.HitPoints, SURGE_DAMAGE);

        targetCombat.ImposeCombatantEffect(targetCombatant,
            new StunCombatantStatus(CombatantStatusSids.Stun, new ToNextCombatantTurnEffectLifetime()));
    }
}
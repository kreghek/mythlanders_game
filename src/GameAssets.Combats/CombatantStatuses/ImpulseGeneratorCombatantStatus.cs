using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class ImpulseGeneratorCombatantStatus : CombatantStatusBase
{
    private const int GENERATED_LIMIT = 2;
    private const int DAMAGE_BONUS = 1;
    private const int SURGE_DAMAGE = 3;

    private readonly ICombatantStatusSid _generatedSid;
    private ICombatant? _statusOwner;

    public ImpulseGeneratorCombatantStatus(ICombatantStatusSid sid, ICombatantStatusSid generatedSid,
        ICombatantStatusLifetime lifetime, ICombatantStatusSource source) : base(sid, lifetime, source)
    {
        _generatedSid = generatedSid;
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        _statusOwner = combatant;

        combatantEffectImposeContext.Combat.CombatantHasChangePosition += Combat_CombatantHasChangePosition;
    }

    private IReadOnlyCollection<ICombatantStatus> CollectImpulseEffects(ICombatant targetCombatant)
    {
        return targetCombatant.Statuses.Where(x => x.Sid == _generatedSid).ToArray();
    }

    private void Combat_CombatantHasChangePosition(object? sender, CombatantHasChangedPositionEventArgs e)
    {
        var targetCombatant = e.Combatant;

        if (targetCombatant != _statusOwner)
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

    private void GainImpulseUnit(ICombatant targetCombatant, CombatEngineBase targetCombat)
    {
        // effect life ends on attack.
        var lifetime = new UntilCombatantEffectMeetPredicatesLifetime(new[]
        {
            new IsAttackCombatMovePredicate()
        });

        var impulseCombatantEffect = new ModifyEffectsCombatantStatus(_generatedSid,
            lifetime, Source, DAMAGE_BONUS);

        targetCombat.ImposeCombatantStatus(targetCombatant, impulseCombatantEffect);
    }

    private void ImpulseSurge(ICombatant targetCombatant, CombatEngineBase targetCombat)
    {
        var impulseEffects = CollectImpulseEffects(targetCombatant);
        foreach (var combatantEffect in impulseEffects)
        {
            targetCombat.DispelCombatantStatus(targetCombatant, combatantEffect);
        }

        targetCombat.HandleCombatantDamagedToStat(targetCombatant, CombatantStatTypes.HitPoints, SURGE_DAMAGE);

        targetCombat.ImposeCombatantStatus(targetCombatant,
            new StunCombatantStatus(CombatantStatusSids.Stun, new ToNextCombatantTurnEffectLifetime(), Source));
    }
}
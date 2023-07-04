using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats.CombatantEffects;

public sealed class ImpulseGeneratorCombatantEffect : CombatantEffectBase
{
    private const int GENERATED_LIMIT = 2;
    private const int DAMAGE_BONUS = 1;
    private const int SURGE_DAMAGE = 3;
    private readonly ICombatantEffectSid _generatedSid;
    private Combatant? _effectOwner;

    public ImpulseGeneratorCombatantEffect(ICombatantEffectSid sid, ICombatantEffectSid generatedSid,
        ICombatantEffectLifetime lifetime) : base(sid, lifetime)
    {
        _generatedSid = generatedSid;
    }

    public override void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        _effectOwner = combatant;

        combatantEffectImposeContext.Combat.CombatantHasChangePosition += Combat_CombatantHasChangePosition;
    }

    private IReadOnlyCollection<ICombatantEffect> CollectImpulseEffects(Combatant targetCombatant)
    {
        return targetCombatant.Effects.Where(x => x.Sid == _generatedSid).ToArray();
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

        var targetCombat = (CombatCore)sender!;

        var currentGeneratedCount = targetCombatant.Effects.Count(x => x.Sid == _generatedSid);

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

    private void ImpulseSurge(Combatant targetCombatant, CombatCore targetCombat)
    {
        var impulseEffects = CollectImpulseEffects(targetCombatant);
        foreach (var combatantEffect in impulseEffects)
        {
            targetCombat.DispelCombatantEffect(targetCombatant, combatantEffect);
        }

        targetCombat.HandleCombatantDamagedToStat(targetCombatant, UnitStatType.HitPoints, SURGE_DAMAGE);

        targetCombat.ImposeCombatantEffect(targetCombatant,
            new StunCombatantEffect(CombatantEffectSids.Stun, new ToNextCombatantTurnEffectLifetime()));
    }

    private void GainImpulseUnit(Combatant targetCombatant, CombatCore targetCombat)
    {
        // effect life ends on attack.
        var lifetime = new UntilCombatantEffectMeetPredicatesLifetime(new[]
        {
            new IsAttackCombatMovePredicate()
        });

        var impulseCombatantEffect = new ModifyEffectsCombatantEffect(_generatedSid,
            lifetime, DAMAGE_BONUS);

        targetCombat.ImposeCombatantEffect(targetCombatant, impulseCombatantEffect);
    }
}
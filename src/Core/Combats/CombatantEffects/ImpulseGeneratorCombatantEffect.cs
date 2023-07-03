using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats.CombatantEffects;

public sealed class ImpulseGeneratorCombatantEffect : CombatantEffectBase
{
    private readonly ICombatantEffectSid _generatedSid;
    private Combatant? _effectOwner;

    private const int GENERATED_LIMIT = 2;
    private const int DAMAGE_BONUS = 1;

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
            ExplodeImpulses(targetCombatant, targetCombat);
        }
    }

    private void ExplodeImpulses(Combatant targetCombatant, CombatCore targetCombat)
    {
        // TODO Drop effects by sid
        var impulseEffects = targetCombatant.Effects.Where(x => x.Sid == _generatedSid);

        // TODO Do damage to targetCombatant
        // TODO Pass turn
    }

    private void GainImpulseUnit(Combatant targetCombatant, CombatCore targetCombat)
    {
        var impulseCombatantEffect = new ModifyEffectsCombatantEffect(_generatedSid,
            new UntilCombatantEffectMeetPredicatesLifetime(new[]
            {
                new IsAttackCombatMovePredicate()
            }), DAMAGE_BONUS);
        
        targetCombat.ImposeCombatantEffect(targetCombatant, impulseCombatantEffect);
    }
}
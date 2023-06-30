using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats.CombatantEffects;

public sealed class ImpulseGeneratorCombatantEffect : CombatantEffectBase
{
    private Combatant? _effectOwner;
    
    public ImpulseGeneratorCombatantEffect(ICombatantEffectLifetime lifetime) : base(lifetime)
    {
    }

    public override void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        _effectOwner = combatant;
        
        combatantEffectImposeContext.Combat.CombatantHasChangePosition += Combat_CombatantHasChangePosition;
    }

    private void Combat_CombatantHasChangePosition(object? sender, CombatantHasChangedPositionEventArgs e)
    {
        if (e.Combatant != _effectOwner)
        {
            // Event handler will raise for every combatants because it subscribed on whole combat.
            // So we must check effect bearer.
            return;
        }

        var targetCombat = (CombatCore)sender!;

        // Impulse effect lives until combatant makes an attacking movement.
        const int DAMAGE_BONUS = 1;
        var impulseCombatantEffect = new ModifyEffectsCombatantEffect(new UntilCombatantEffectMeetPredicatesLifetime(new[]
        {
            new IsAttackCombatMovePredicate()
        }), DAMAGE_BONUS);
        
        e.Combatant.AddEffect(impulseCombatantEffect,
            new CombatantEffectImposeContext(targetCombat),
            new CombatantEffectLifetimeImposeContext(e.Combatant, targetCombat));
    }
}
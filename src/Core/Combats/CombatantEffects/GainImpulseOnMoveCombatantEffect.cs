using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats.CombatantEffects;

public sealed class GainImpulseOnMoveCombatantEffect : CombatantEffectBase
{
    public GainImpulseOnMoveCombatantEffect(ICombatantEffectLifetime lifetime) : base(lifetime)
    {
    }

    public override void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);
        
        combatantEffectImposeContext.Combat.CombatantHasChangePosition += Combat_CombatantHasChangePosition;
    }

    private static void Combat_CombatantHasChangePosition(object? sender, CombatantHasChangedPositionEventArgs e)
    {
        var targetCombat = (CombatCore)sender!;

        // Impulse effect lives until combatant makes an attacking movement.
        var impulseCombatantEffect = new ImpulseCombatantEffect(new UntilCombatantEffectMeetPredicatesLifetime(new[]
        {
            new IsAttackCombatMovePredicate()
        }));
        
        e.Combatant.AddEffect(impulseCombatantEffect,
            new CombatantEffectImposeContext(targetCombat),
            new CombatantEffectLifetimeImposeContext(targetCombat));
    }
}

public sealed class GainImpulseOnMoveCombatantEffectFactory : ICombatantEffectFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;

    public GainImpulseOnMoveCombatantEffectFactory(ICombatantEffectLifetimeFactory lifetimeFactory)
    {
        _lifetimeFactory = lifetimeFactory;
    }

    public ICombatantEffect Create()
    {
        return new GainImpulseOnMoveCombatantEffect(_lifetimeFactory.Create());
    }
}
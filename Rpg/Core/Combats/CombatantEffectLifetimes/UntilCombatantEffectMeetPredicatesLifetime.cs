namespace Core.Combats.CombatantEffectLifetimes;

public sealed class UntilCombatantEffectMeetPredicatesLifetime : ICombatantEffectLifetime
{
    private readonly IReadOnlyCollection<ICombatMovePredicate> _combatMovePredicates;

    public UntilCombatantEffectMeetPredicatesLifetime(IReadOnlyCollection<ICombatMovePredicate> combatMovePredicates)
    {
        _combatMovePredicates = combatMovePredicates;
    }

    private void Combat_CombatantUsedMove(object? sender, CombatantHandChangedEventArgs e)
    {
        if (_combatMovePredicates.All(x => x.Check(e.Move))) IsDead = true;
    }

    public bool IsDead { get; private set; }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
    }

    public void EffectImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
        context.Combat.CombatantUsedMove += Combat_CombatantUsedMove;
    }

    public void EffectDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
        context.Combat.CombatantUsedMove -= Combat_CombatantUsedMove;
    }
}
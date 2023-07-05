namespace Core.Combats.CombatantEffectLifetimes;

public sealed class UntilCombatantEffectMeetPredicatesLifetime : ICombatantEffectLifetime
{
    private readonly IReadOnlyCollection<ICombatMovePredicate> _combatMovePredicates;
    private Combatant? _ownedCombatant;

    public UntilCombatantEffectMeetPredicatesLifetime(IReadOnlyCollection<ICombatMovePredicate> combatMovePredicates)
    {
        _combatMovePredicates = combatMovePredicates;
    }

    private void Combat_CombatantUsedMove(object? sender, CombatantHandChangedEventArgs e)
    {
        if (_ownedCombatant != e.Combatant)
        {
            // Check only if owner performs combat movements.
            return;
        }

        if (_combatMovePredicates.All(x => x.Check(e.Move)))
        {
            IsExpired = true;
        }
    }

    public bool IsExpired { get; private set; }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
    }

    public void HandleOwnerImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
        _ownedCombatant = context.TargetCombatant;
        context.Combat.CombatantUsedMove += Combat_CombatantUsedMove;
    }

    public void HandleOwnerDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
        context.Combat.CombatantUsedMove -= Combat_CombatantUsedMove;
    }
}
namespace Core.Combats.CombatantEffectLifetimes;

public sealed class MultipleCombatantTurnEffectLifetime : ICombatantEffectLifetime
{
    private bool _currentRoundEnd;

    public MultipleCombatantTurnEffectLifetime(int duration)
    {
        Counter = duration;
    }

    public int Counter { get; set; }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        if (updateType == CombatantEffectUpdateType.EndRound) _currentRoundEnd = true;

        if (_currentRoundEnd && updateType == CombatantEffectUpdateType.EndRound)
        {
            Counter--;
            if (Counter == 0) IsDead = true;
        }
    }

    public void EffectImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
    }

    public void EffectDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
    }

    public bool IsDead { get; private set; }
}

public interface ICombatMovePredicate
{
    bool Check(CombatMovementInstance combatMove);
}

public sealed class IsAttackCombatMovePredicate: ICombatMovePredicate
{
    public bool Check(CombatMovementInstance combatMove)
    {
        return combatMove.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack);
    }
}

public sealed class UntilCombatantEffectMeetPredicatesLifetime : ICombatantEffectLifetime
{
    private readonly IReadOnlyCollection<ICombatMovePredicate> _combatMovePredicates;

    public UntilCombatantEffectMeetPredicatesLifetime(IReadOnlyCollection<ICombatMovePredicate> combatMovePredicates)
    {
        _combatMovePredicates = combatMovePredicates;
    }

    public bool IsDead { get; private set; }
    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
    }

    public void EffectImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
        context.Combat.CombatantUsedMove += Combat_CombatantUsedMove;
    }

    private void Combat_CombatantUsedMove(object? sender, CombatantHandChangedEventArgs e)
    {
        if (_combatMovePredicates.All(x => x.Check(e.Move)))
        {
            IsDead = true;
        }
    }

    public void EffectDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
        context.Combat.CombatantUsedMove -= Combat_CombatantUsedMove;
    }
}

public sealed class UntilUseMoveCombatantEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    private readonly IReadOnlyCollection<ICombatMovePredicate> _combatMovePredicates;

    public UntilUseMoveCombatantEffectLifetimeFactory(params ICombatMovePredicate[] combatMovePredicates)
    {
        _combatMovePredicates = combatMovePredicates;
    }

    public ICombatantEffectLifetime Create()
    {
        return new UntilCombatantEffectMeetPredicatesLifetime(_combatMovePredicates);
    }
}
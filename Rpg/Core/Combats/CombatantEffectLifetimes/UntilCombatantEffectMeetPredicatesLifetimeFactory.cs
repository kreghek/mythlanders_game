namespace Core.Combats.CombatantEffectLifetimes;

public sealed class UntilCombatantEffectMeetPredicatesLifetimeFactory : ICombatantEffectLifetimeFactory
{
    private readonly IReadOnlyCollection<ICombatMovePredicate> _combatMovePredicates;

    public UntilCombatantEffectMeetPredicatesLifetimeFactory(params ICombatMovePredicate[] combatMovePredicates)
    {
        _combatMovePredicates = combatMovePredicates;
    }

    public ICombatantEffectLifetime Create()
    {
        return new UntilCombatantEffectMeetPredicatesLifetime(_combatMovePredicates);
    }
}
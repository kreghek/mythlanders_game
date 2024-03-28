using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class OwnerIsAttacked : ICombatMovePredicate
{
    public bool Check(CombatMovementInstance combatMove)
    {
        //TODO Implement
        return false;
    }
}

public sealed class OwnerStatBelow : ICombatMovePredicate
{
    public bool Check(CombatMovementInstance combatMove)
    {
        //TODO Implement
        return false;
    }
}
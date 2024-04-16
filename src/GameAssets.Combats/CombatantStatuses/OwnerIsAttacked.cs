using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class OwnerIsAttacked : ICombatantStatusLifetimeExpirationCondition
{
    public bool Check(CombatMovementInstance combatMove)
    {
        //TODO Implement
        return false;
    }
}
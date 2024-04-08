using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class OwnerIsAttackedOrMovedLifetimeExpirationCondition : ICombatantStateLifetimeExpirationCondition
{
    public bool Check(ICombatant statusOwner, CombatEngineBase combatEngine)
    {
        // Status expires then a combatant is attacked or moved.
        return true;
    }
}
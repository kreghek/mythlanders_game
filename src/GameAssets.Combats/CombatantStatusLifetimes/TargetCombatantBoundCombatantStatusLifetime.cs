using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatusLifetimes;

public class TargetCombatantBoundCombatantStatusLifetime : ICombatantStatusLifetime
{
    public TargetCombatantBoundCombatantStatusLifetime(ICombatant combatant)
    {
        throw new NotImplementedException();
    }

    public void HandleDispelling(ICombatantStatus owner, ICombatantStatusLifetimeDispelContext context)
    {
        throw new NotImplementedException();
    }

    public void HandleImposed(ICombatantStatus owner, ICombatantStatusLifetimeImposeContext context)
    {
        throw new NotImplementedException();
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        throw new NotImplementedException();
    }

    public bool IsExpired { get; }
}
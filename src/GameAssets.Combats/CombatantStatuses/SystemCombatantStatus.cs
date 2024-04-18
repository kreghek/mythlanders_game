using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatuses;

public abstract class SystemCombatantStatus:ICombatantStatus
{
    public virtual void Dispel(ICombatant combatant)
    {
    }

    public virtual void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
    }

    public virtual void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
    }
    public abstract ICombatantStatusLifetime Lifetime { get; }
    public abstract ICombatantStatusSid Sid { get; }
    public abstract ICombatantStatusSource Source { get; }
}
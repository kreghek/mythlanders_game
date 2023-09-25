using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace Core.Combats.CombatantStatuses;

public sealed class ConterAttackCombatantStatus : CombatantStatusBase
{
    public ConterAttackCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
    }
}

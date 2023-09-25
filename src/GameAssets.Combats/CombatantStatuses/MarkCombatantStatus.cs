using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace Core.Combats.CombatantStatuses;

public sealed class MarkCombatantStatus : CombatantStatusBase
{
    public MarkCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
    }
}

namespace Core.Combats.CombatantStatus;

public sealed class MarkCombatantStatus : CombatantStatusBase
{
    public MarkCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
    }
}
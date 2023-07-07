namespace Core.Combats.CombatantStatus;

/// <summary>
/// Combatant with tis condition will pass next turn.
/// </summary>
public class StunCombatantStatus : ICombatantStatus
{
    public StunCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime)
    {
        Lifetime = lifetime;
        Sid = sid;
    }

    public ICombatantStatusLifetime Lifetime { get; }
    public ICombatantStatusSid Sid { get; }

    public void Dispel(Combatant combatant)
    {
        // Do nothing
    }

    public void Impose(Combatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        // Do nothing
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        if (updateType == CombatantStatusUpdateType.StartCombatantTurn)
        {
            context.CompleteTurn();
        }
    }
}
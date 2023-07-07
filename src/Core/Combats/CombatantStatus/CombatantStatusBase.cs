namespace Core.Combats.CombatantStatus;

public abstract class CombatantStatusBase : ICombatantStatus
{
    protected CombatantStatusBase(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime)
    {
        Sid = sid;
        Lifetime = lifetime;
    }

    public ICombatantStatusSid Sid { get; }
    public ICombatantStatusLifetime Lifetime { get; }

    public virtual void Dispel(Combatant combatant)
    {
    }

    public virtual void Impose(Combatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}
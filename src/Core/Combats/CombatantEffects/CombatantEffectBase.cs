namespace Core.Combats.CombatantEffects;

public abstract class CombatantEffectBase : ICombatantEffect
{
    protected CombatantEffectBase(ICombatantEffectSid sid, ICombatantEffectLifetime lifetime)
    {
        Sid = sid;
        Lifetime = lifetime;
    }

    public ICombatantEffectSid Sid { get; }
    public ICombatantEffectLifetime Lifetime { get; }

    public virtual void Dispel(Combatant combatant)
    {
    }

    public virtual void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext)
    {
    }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}
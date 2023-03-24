namespace Core.Combats.CombatantEffects;

public abstract class CombatantEffectBase : ICombatantEffect
{
    public ICombatantEffectLifetime Lifetime { get; }
    public virtual void Dispel(Combatant combatant)
    {
    }

    public virtual void Impose(Combatant combatant)
    {
    }

    protected CombatantEffectBase(ICombatantEffectLifetime lifetime)
    {
        Lifetime = lifetime;
    }
    
    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}
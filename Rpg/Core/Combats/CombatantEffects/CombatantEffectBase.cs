namespace Core.Combats.CombatantEffects;

public abstract class CombatantEffectBase : ICombatantEffect
{
    protected CombatantEffectBase(ICombatantEffectLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    public ICombatantEffectLifetime Lifetime { get; }

    public virtual void Dispel(Combatant combatant)
    {
    }

    public virtual void Impose(Combatant combatant)
    {
    }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}
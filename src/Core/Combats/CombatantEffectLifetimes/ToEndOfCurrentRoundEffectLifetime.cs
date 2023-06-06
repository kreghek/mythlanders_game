namespace Core.Combats.CombatantEffectLifetimes;

public sealed class ToEndOfCurrentRoundEffectLifetime : ICombatantEffectLifetime
{
    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        if (updateType == CombatantEffectUpdateType.EndRound)
        {
            IsDead = true;
        }
    }

    public void EffectImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
    }

    public void EffectDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
    }

    public bool IsDead { get; private set; }
}

public sealed class ToEndOfCurrentRoundEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    public ICombatantEffectLifetime Create()
    {
        return new ToEndOfCurrentRoundEffectLifetime();
    }
}
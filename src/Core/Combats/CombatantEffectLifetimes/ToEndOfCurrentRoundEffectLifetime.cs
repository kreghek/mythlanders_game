namespace Core.Combats.CombatantEffectLifetimes;

public sealed class ToEndOfCurrentRoundEffectLifetime : ICombatantEffectLifetime
{
    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        if (updateType == CombatantEffectUpdateType.EndRound)
        {
            IsExpired = true;
        }
    }

    public void HandleOwnerImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
    }

    public void HandleOwnerDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
    }

    public bool IsExpired { get; private set; }
}

public sealed class ToEndOfCurrentRoundEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    public ICombatantEffectLifetime Create()
    {
        return new ToEndOfCurrentRoundEffectLifetime();
    }
}
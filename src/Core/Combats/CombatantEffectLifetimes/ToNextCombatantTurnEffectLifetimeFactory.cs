namespace Core.Combats.CombatantEffectLifetimes;

public sealed class ToNextCombatantTurnEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new ToNextCombatantTurnEffectLifetime();
    }
}
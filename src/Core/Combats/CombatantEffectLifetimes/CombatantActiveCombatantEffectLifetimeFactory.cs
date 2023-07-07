namespace Core.Combats.CombatantEffectLifetimes;

public sealed class CombatantActiveCombatantEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new CombatantActiveCombatantEffectLifetime();
    }
}
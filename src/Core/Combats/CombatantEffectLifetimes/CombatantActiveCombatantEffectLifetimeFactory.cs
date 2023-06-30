namespace Core.Combats.CombatantEffectLifetimes;

public sealed class CombatantActiveCombatantEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    public ICombatantEffectLifetime Create()
    {
        return new CombatantActiveCombatantEffectLifetime();
    }
}
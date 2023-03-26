namespace Core.Combats.CombatantEffectLifetimes;

public sealed class ToNextCombatantTurnEffectLifetimeFactory: ICombatantEffectLifetimeFactory
{
    public ICombatantEffectLifetime Create()
    {
        return new ToNextCombatantTurnEffectLifetime();
    }
}
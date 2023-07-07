namespace Core.Combats;

public interface ICombatantEffectLifetimeFactory
{
    ICombatantStatusLifetime Create();
}
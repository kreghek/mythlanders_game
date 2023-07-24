using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats;

public sealed class MultipleCombatantTurnEffectLifetimeFactory : ICombatantStatusLifetimeFactory
{
    private readonly int _turnCount;

    public MultipleCombatantTurnEffectLifetimeFactory(int turnCount)
    {
        _turnCount = turnCount;
    }

    public ICombatantStatusLifetime Create()
    {
        return new MultipleCombatantTurnEffectLifetime(_turnCount);
    }
}
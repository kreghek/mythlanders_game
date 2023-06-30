using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats;

public sealed class MultipleCombatantTurnEffectLifetimeFactory : ICombatantEffectLifetimeFactory
{
    private readonly int _turnCount;

    public MultipleCombatantTurnEffectLifetimeFactory(int turnCount)
    {
        _turnCount = turnCount;
    }

    public ICombatantEffectLifetime Create()
    {
        return new MultipleCombatantTurnEffectLifetime(_turnCount);
    }
}
using Core.Combats.CombatantEffectLifetimes;

namespace Core.Combats;

public interface ICombatantEffectLifetime
{
    bool IsDead { get; }
    void EffectDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context);
    void EffectImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context);
    void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context);
}

public interface ICombatantEffectLifetimeFactory
{
    ICombatantEffectLifetime Create();
}

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
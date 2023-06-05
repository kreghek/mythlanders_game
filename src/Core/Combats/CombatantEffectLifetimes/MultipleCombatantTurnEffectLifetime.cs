namespace Core.Combats.CombatantEffectLifetimes;

public sealed class MultipleCombatantTurnEffectLifetime : ICombatantEffectLifetime
{
    private bool _currentRoundEnd;

    public MultipleCombatantTurnEffectLifetime(int duration)
    {
        Counter = duration;
    }

    public int Counter { get; set; }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        if (updateType == CombatantEffectUpdateType.EndRound) _currentRoundEnd = true;

        if (_currentRoundEnd && updateType == CombatantEffectUpdateType.EndRound)
        {
            Counter--;
            if (Counter == 0) IsDead = true;
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
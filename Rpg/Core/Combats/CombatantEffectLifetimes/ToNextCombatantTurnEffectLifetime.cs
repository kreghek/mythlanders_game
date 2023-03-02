namespace Core.Combats.CombatantEffectLifetimes;

public sealed class ToNextCombatantTurnEffectLifetime : ICombatantEffectLifetime
{
    private bool _currentRoundEnd;

    public void Update(CombatantEffectUpdateType updateType)
    {
        if (updateType == CombatantEffectUpdateType.EndRound) _currentRoundEnd = true;

        if (_currentRoundEnd && updateType == CombatantEffectUpdateType.StartCombatantTurn) IsDead = true;
    }

    public bool IsDead { get; private set; }
}

public sealed class MultipleCombatantTurnEffectLifetime : ICombatantEffectLifetime
{
    private int _counter;
    private bool _currentRoundEnd;

    public MultipleCombatantTurnEffectLifetime(int duration)
    {
        _counter = duration;
    }

    public void Update(CombatantEffectUpdateType updateType)
    {
        if (updateType == CombatantEffectUpdateType.EndRound) _currentRoundEnd = true;

        if (_currentRoundEnd && updateType == CombatantEffectUpdateType.EndRound)
        {
            _counter--;
            if (_counter == 0) IsDead = true;
        }
    }

    public bool IsDead { get; private set; }
}
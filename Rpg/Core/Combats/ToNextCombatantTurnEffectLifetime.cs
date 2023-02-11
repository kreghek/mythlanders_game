namespace Core.Combats;

public sealed class ToNextCombatantTurnEffectLifetime : ICombatantEffectLifetime
{
    private bool _currentRoundEnd;
    public void Update(CombatantEffectUpdateType updateType)
    {
        if (updateType == CombatantEffectUpdateType.EndRound)
        {
            _currentRoundEnd = true;
        }

        if (_currentRoundEnd && updateType == CombatantEffectUpdateType.StartCombatantTurn)
        {
            IsDead = true;
        }
    }

    public bool IsDead { get; private set; }
}
namespace Core.Combats.CombatantEffectLifetimes;

public sealed class ToNextCombatantTurnEffectLifetime : ICombatantEffectLifetime
{
    private bool _currentRoundEnd;

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        if (updateType == CombatantEffectUpdateType.EndRound)
        {
            _currentRoundEnd = true;
        }

        if (_currentRoundEnd && updateType == CombatantEffectUpdateType.StartCombatantTurn)
        {
            IsExpired = true;
        }
    }

    public void HandleOwnerImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
    }

    public void HandleOwnerDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
    }

    public bool IsExpired { get; private set; }
}
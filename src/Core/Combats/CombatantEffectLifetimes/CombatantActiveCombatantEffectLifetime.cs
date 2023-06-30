namespace Core.Combats.CombatantEffectLifetimes;

public sealed class CombatantActiveCombatantEffectLifetime: ICombatantEffectLifetime
{
    private readonly Combatant _monitoringTarget;

    public CombatantActiveCombatantEffectLifetime(Combatant monitoringTarget)
    {
        _monitoringTarget = monitoringTarget;
    }

    private void CombatCore_CombatantHasBeenDefeated(object? sender, CombatantDefeatedEventArgs e)
    {
        if (e.Combatant == _monitoringTarget)
        {
            // Bearer of effect is dead.
            // So effect can't do anything.

            IsExpired = true;
        }
    }

    /// <inheritdoc/>
    public bool IsExpired { get; private set; }
    
    public void HandleOwnerDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
        context.Combat.CombatantHasBeenDefeated -= CombatCore_CombatantHasBeenDefeated;
    }

    public void HandleOwnerImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
        context.Combat.CombatantHasBeenDefeated += CombatCore_CombatantHasBeenDefeated;
    }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
    }
}
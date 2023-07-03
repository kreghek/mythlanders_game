namespace Core.Combats.CombatantEffectLifetimes;

public sealed class CombatantActiveCombatantEffectLifetime : ICombatantEffectLifetime
{
    private void CombatCore_CombatantHasBeenDefeated(object? sender, CombatantDefeatedEventArgs e)
    {
        IsExpired = true;
    }

    /// <inheritdoc />
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
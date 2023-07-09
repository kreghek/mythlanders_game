namespace Core.Combats;

public sealed class CombatantEffectLifetimeImposeContext : ICombatantStatusLifetimeImposeContext
{
    public CombatantEffectLifetimeImposeContext(Combatant targetCombatant, CombatCore combat)
    {
        TargetCombatant = targetCombatant;
        Combat = combat;
    }

    public CombatCore Combat { get; }
    public Combatant TargetCombatant { get; }
}
namespace Core.Combats;

public sealed class CombatantEffectLifetimeImposeContext : ICombatantStatusLifetimeImposeContext
{
    public CombatantEffectLifetimeImposeContext(Combatant targetCombatant, CombatEngineBase combat)
    {
        TargetCombatant = targetCombatant;
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }
    public Combatant TargetCombatant { get; }
}
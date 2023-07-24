namespace Core.Combats;

public interface ICombatantStatusLifetimeImposeContext
{
    CombatEngineBase Combat { get; }
    Combatant TargetCombatant { get; }
}
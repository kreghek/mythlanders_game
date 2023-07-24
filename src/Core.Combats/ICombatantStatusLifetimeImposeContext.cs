namespace Core.Combats;

public interface ICombatantStatusLifetimeImposeContext
{
    CombatEngineBase Combat { get; }
    ICombatant TargetCombatant { get; }
}
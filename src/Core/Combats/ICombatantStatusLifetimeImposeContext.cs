namespace Core.Combats;

public interface ICombatantStatusLifetimeImposeContext
{
    CombatCore Combat { get; }
    Combatant TargetCombatant { get; }
}

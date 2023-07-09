namespace Core.Combats;

public interface ICombatantStatusLifetimeUpdateContext
{
    Combatant Combatant { get; }
    void CompleteTurn();
}
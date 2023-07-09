namespace Core.Combats;

public sealed record CombatantEffectLifetimeUpdateContext
    (Combatant Combatant, CombatCore Combat) : ICombatantStatusLifetimeUpdateContext
{
    public void CompleteTurn()
    {
        Combat.Interrupt();
    }
}
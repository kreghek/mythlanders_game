namespace Core.Combats;

public sealed record CombatantEffectLifetimeUpdateContext
    (Combatant Combatant, CombatEngineBase Combat) : ICombatantStatusLifetimeUpdateContext
{
    public void CompleteTurn()
    {
        Combat.Interrupt();
    }
}
namespace Core.Combats;

public sealed record CombatantEffectLifetimeUpdateContext(Combatant Combatant, CombatCore Combat) : ICombatantEffectLifetimeUpdateContext
{
    public void CompleteTurn()
    {
        Combat.Interrupt();
    }
}
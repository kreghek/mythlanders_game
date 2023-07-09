namespace Core.Combats;

public sealed class CombatantEffectLifetimeDispelContext : ICombatantStatusLifetimeDispelContext
{
    public CombatantEffectLifetimeDispelContext(CombatCore combat)
    {
        Combat = combat;
    }

    public CombatCore Combat { get; }
}
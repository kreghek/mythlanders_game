namespace Core.Combats;

public sealed class CombatantEffectLifetimeDispelContext : ICombatantEffectLifetimeDispelContext
{
    public CombatantEffectLifetimeDispelContext(CombatCore combat)
    {
        Combat = combat;
    }

    public CombatCore Combat { get; }
}
namespace Core.Combats;

public sealed class CombatantEffectLifetimeImposeContext : ICombatantEffectLifetimeImposeContext
{
    public CombatantEffectLifetimeImposeContext(CombatCore combat)
    {
        Combat = combat;
    }

    public CombatCore Combat { get; }
}

public sealed class CombatantEffectImposeContext : ICombatantEffectImposeContext
{
    public CombatantEffectImposeContext(CombatCore combat)
    {
        Combat = combat;
    }

    public CombatCore Combat { get; }
}
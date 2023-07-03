namespace Core.Combats;

public sealed class CombatantEffectImposeContext : ICombatantEffectImposeContext
{
    public CombatantEffectImposeContext(CombatCore combat)
    {
        Combat = combat;
    }

    public CombatCore Combat { get; }
}
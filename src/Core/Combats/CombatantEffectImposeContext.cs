namespace Core.Combats;

public sealed class CombatantEffectImposeContext : ICombatantStatusImposeContext
{
    public CombatantEffectImposeContext(CombatCore combat)
    {
        Combat = combat;
    }

    public CombatCore Combat { get; }
}
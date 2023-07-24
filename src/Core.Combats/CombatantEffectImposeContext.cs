namespace Core.Combats;

public sealed class CombatantEffectImposeContext : ICombatantStatusImposeContext
{
    public CombatantEffectImposeContext(CombatEngineBase combat)
    {
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }
}
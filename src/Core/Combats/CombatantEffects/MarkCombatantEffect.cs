namespace Core.Combats.CombatantEffects;

public sealed class MarkCombatantEffect : CombatantEffectBase
{
    public MarkCombatantEffect(ICombatantEffectLifetime lifetime) : base(lifetime)
    {
    }
}

public sealed class ImpulseCombatantEffect : CombatantEffectBase
{
    public ImpulseCombatantEffect(ICombatantEffectLifetime lifetime) : base(lifetime)
    {
    }
}
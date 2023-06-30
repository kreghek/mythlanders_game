namespace Core.Combats.CombatantEffects;

public sealed class MarkCombatantEffect : CombatantEffectBase
{
    public MarkCombatantEffect(ICombatantEffectSid sid, ICombatantEffectLifetime lifetime) : base(sid, lifetime)
    {
    }
}
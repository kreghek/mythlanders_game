namespace Core.Combats;

public static class CombatantEffectSids
{
    public static ICombatantEffectSid Impulse { get; } = new CombatantEffectSid(nameof(Impulse));
    public static ICombatantEffectSid ImpulseGenerator { get; } = new CombatantEffectSid(nameof(ImpulseGenerator));
    public static ICombatantEffectSid Mark { get; } = new CombatantEffectSid(nameof(Mark));
}
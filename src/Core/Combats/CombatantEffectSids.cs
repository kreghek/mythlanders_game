namespace Core.Combats;

public static class CombatantEffectSids
{
    public static ICombatantStatusSid Impulse { get; } = new CombatantEffectSid(nameof(Impulse));
    public static ICombatantStatusSid ImpulseGenerator { get; } = new CombatantEffectSid(nameof(ImpulseGenerator));
    public static ICombatantStatusSid Mark { get; } = new CombatantEffectSid(nameof(Mark));

    public static ICombatantStatusSid Stun { get; } = new CombatantEffectSid(nameof(Stun));
}
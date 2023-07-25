using CombatDicesTeam.Combats;

namespace GameAssets.Combats;

public static class CombatantStatusSids
{
    public static ICombatantStatusSid Impulse { get; } = new CombatantEffectSid(nameof(Impulse));
    public static ICombatantStatusSid ImpulseGenerator { get; } = new CombatantEffectSid(nameof(ImpulseGenerator));
    public static ICombatantStatusSid Mark { get; } = new CombatantEffectSid(nameof(Mark));

    public static ICombatantStatusSid Stun { get; } = new CombatantEffectSid(nameof(Stun));
}
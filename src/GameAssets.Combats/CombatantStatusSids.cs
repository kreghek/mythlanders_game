using CombatDicesTeam.Combats;

namespace GameAssets.Combats;

public static class CombatantStatusSids
{
    public static ICombatantStatusSid Impulse { get; } = new CombatantStatusSid(nameof(Impulse));
    public static ICombatantStatusSid ImpulseGenerator { get; } = new CombatantStatusSid(nameof(ImpulseGenerator));
    public static ICombatantStatusSid Mark { get; } = new CombatantStatusSid(nameof(Mark));

    public static ICombatantStatusSid Stun { get; } = new CombatantStatusSid(nameof(Stun));
}
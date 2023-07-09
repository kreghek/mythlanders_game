namespace Core.Combats;

public static class CombatantStatTypes
{
    public static ICombatantStatType Defense { get; } = new CombatantStatType();
    public static ICombatantStatType HitPoints { get; } = new CombatantStatType();
    public static ICombatantStatType Maneuver { get; } = new CombatantStatType();
    public static ICombatantStatType Resolve { get; } = new CombatantStatType();
    public static ICombatantStatType ShieldPoints { get; } = new CombatantStatType();
}
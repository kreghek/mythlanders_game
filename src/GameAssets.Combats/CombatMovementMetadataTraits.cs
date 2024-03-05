namespace GameAssets.Combats;

public static class CombatMovementMetadataTraits
{
    public static CombatMovementMetadataTrait Melee { get; } = new CombatMovementMetadataTrait(nameof(Melee));
    public static CombatMovementMetadataTrait Ranged { get; } = new CombatMovementMetadataTrait(nameof(Ranged));
    public static CombatMovementMetadataTrait Support { get; } = new CombatMovementMetadataTrait(nameof(Support));
}
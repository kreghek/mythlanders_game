namespace GameAssets.Combats;

public static class CombatMovementMetadataTraits
{
    public static CombatMovementMetadataTrait Melee { get; } = new CombatMovementMetadataTrait(nameof(Melee));
    public static CombatMovementMetadataTrait Range { get; } = new CombatMovementMetadataTrait(nameof(Range));
}

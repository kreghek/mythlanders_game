using Core.Combats;

namespace GameAssets.Combats;

public static class CombatMovementContainerTypes
{
    public static ICombatMovementContainerType Hand { get; } = new CombatMovementContainerType(nameof(Hand));
    public static ICombatMovementContainerType Pool { get; } = new CombatMovementContainerType(nameof(Pool));
}
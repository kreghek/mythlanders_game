using System.Collections.Generic;

namespace Client.GameScreens.Combat;

internal sealed record CombatRewards
{
    public ResourceReward BiomeProgress { get; init; }
    public IReadOnlyCollection<ResourceReward> InventoryRewards { get; init; }
}
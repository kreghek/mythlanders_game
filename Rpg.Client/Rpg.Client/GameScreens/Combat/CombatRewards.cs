using System.Collections.Generic;

namespace Rpg.Client.GameScreens.Combat
{
    internal sealed record CombatRewards
    {
        public ProgressionRewardStat BiomeProgress { get; init; }
        public IReadOnlyCollection<ResourceReward> InventoryRewards { get; init; }
    }
}
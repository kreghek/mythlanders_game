using System.Collections.Generic;

using Client.GameScreens.Combat;

namespace Rpg.Client.GameScreens.Combat
{
    internal sealed record CombatRewards
    {
        public ResourceReward BiomeProgress { get; init; }
        public IReadOnlyCollection<ResourceReward> InventoryRewards { get; init; }
    }
}
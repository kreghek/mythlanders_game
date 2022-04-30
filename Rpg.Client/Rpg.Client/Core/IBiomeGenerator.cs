using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IBiomeGenerator
    {
        void CreateStartCombat(Biome startBiome);

        void CreateCombatsInBiomeNodes(IEnumerable<Biome> biomes, GlobeLevel globeLevel);

        IReadOnlyList<Biome> GenerateStartState();
    }
}
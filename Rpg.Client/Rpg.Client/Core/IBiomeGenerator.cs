using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IBiomeGenerator
    {
        void CreateCombatsInBiomeNodes(IEnumerable<Biome> biomes, GlobeLevel globeLevel);

        IReadOnlyList<Biome> Generate();
    }
}
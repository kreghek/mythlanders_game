using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal class MonsterGenerationGlobeContext : IMonsterGenerationGlobeContext
    {
        public MonsterGenerationGlobeContext(GlobeLevel globeLevel
            /*IEnumerable<Biome> biomes*/)
        {
            GlobeProgressLevel = globeLevel.Level;
            //BiomesWithBosses = biomes.Where(x => !x.IsComplete).Select(x => x.Type).ToArray();
        }

        public int GlobeProgressLevel { get; }
        public IReadOnlyCollection<BiomeType> BiomesWithBosses { get; }
    }
}
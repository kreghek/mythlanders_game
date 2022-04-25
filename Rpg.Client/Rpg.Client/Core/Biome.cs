using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Biome
    {
        public Biome(BiomeType type)
        {
            Type = type;
        }

        public bool IsAvailable { get; set; }

        public bool IsComplete { get; set; }

        public IEnumerable<GlobeNode> Nodes { get; init; }

        public BiomeType Type { get; }
    }
}
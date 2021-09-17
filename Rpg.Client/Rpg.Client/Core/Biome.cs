using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Biome
    {
        public Biome(int minLevel, BiomeType type)
        {
            MinLevel = minLevel;
            Type = type;

            Level = MinLevel;
        }

        public bool IsAvailable { get; set; }

        public bool IsComplete { get; set; }
        public bool IsFinal { get; set; }

        public bool IsStart { get; set; }

        public int Level { get; set; }

        public int MinLevel { get; }

        public IEnumerable<GlobeNode> Nodes { get; init; }
        public BiomeType Type { get; }

        public BiomeType? UnlockBiome { get; internal set; }
    }
}
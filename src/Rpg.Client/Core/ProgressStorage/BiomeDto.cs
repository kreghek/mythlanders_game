using System.Collections.Generic;

namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record BiomeDto
    {
        public bool IsComplete { get; init; }
        public IEnumerable<GlobeNodeDto?>? Nodes { get; init; }
        public BiomeType Type { get; init; }
    }
}
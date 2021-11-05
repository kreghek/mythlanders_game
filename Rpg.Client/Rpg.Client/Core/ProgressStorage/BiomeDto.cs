using System.Collections.Generic;

namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record BiomeDto
    {
        public bool IsAvailable { get; init; }
        public bool IsComplete { get; init; }
        public int Level { get; init; }
        public IEnumerable<GlobeNodeDto?>? Nodes { get; init; }
        public BiomeType Type { get; init; }
    }

    internal sealed record GlobeNodeDto
    {
        public bool IsAvailable { get; init; }
        public GlobeNodeSid Sid { get; init; }
    }
}
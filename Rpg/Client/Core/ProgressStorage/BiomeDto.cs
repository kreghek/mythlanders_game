using System.Collections.Generic;

using Client.Assets;

namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record BiomeDto
    {
        public bool IsComplete { get; init; }
        public IEnumerable<GlobeNodeDto?>? Nodes { get; init; }
        public BiomeCulture Type { get; init; }
    }
}
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Event
    {
        public string Name { get; set; }
        public int Counter { get; set; }
        public bool IsUnique { get; set; }
        public IEnumerable<EventNode> Nodes { get; init; }
        public EventNode StartNode { get; init; }

        public SystemEventMarker? SystemMarker { get; set; }
        public BiomeType? Biome { get; internal set; }
        public int? RequiredBiomeLevel { get; internal set; }
    }
}
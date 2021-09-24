﻿using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Event
    {
        public BiomeType? Biome { get; internal set; }
        public bool Completed { get; set; }
        public int Counter { get; set; }
        public bool IsUnique { get; set; }
        public string Name { get; set; }
        public IEnumerable<EventNode> Nodes { get; init; }
        public int? RequiredBiomeLevel { get; internal set; }
        public string?[]? RequiredEventsCompleted { get; internal set; }
        public EventNode StartNode { get; init; }
        public SystemEventMarker? SystemMarker { get; set; }
    }
}
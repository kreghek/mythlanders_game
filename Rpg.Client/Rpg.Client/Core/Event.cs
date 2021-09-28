namespace Rpg.Client.Core
{
    internal sealed class Event
    {
        public EventNode AfterCombatStartNode { get; init; }
        public EventNode BeforeCombatStartNode { get; init; }
        public BiomeType? Biome { get; internal set; }
        public bool Completed { get; set; }
        public int Counter { get; set; }
        public bool IsUnique { get; set; }
        public string Name { get; set; }
        public int? RequiredBiomeLevel { get; internal set; }
        public string?[]? RequiredEventsCompleted { get; internal set; }
        public SystemEventMarker? SystemMarker { get; set; }
    }
}
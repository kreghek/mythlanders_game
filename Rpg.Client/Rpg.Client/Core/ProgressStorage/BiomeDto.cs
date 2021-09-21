namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record BiomeDto
    { 
        public BiomeType Type { get; init; }
        public int Level { get; init; }
        public bool IsComplete { get; internal set; }
        public bool IsAvailable { get; internal set; }
    }
}
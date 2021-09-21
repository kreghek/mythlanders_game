namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record BiomeDto
    {
        public bool IsAvailable { get; init; }
        public bool IsComplete { get; init; }
        public int Level { get; init; }
        public BiomeType Type { get; init; }
    }
}
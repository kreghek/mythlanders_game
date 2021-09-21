namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record BiomeDto
    {
        public BiomeType Type { get; init; }
        public int Level { get; init; }
        public bool IsComplete { get; init; }
        public bool IsAvailable { get; init; }
    }
}
namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record ResourceDto
    {
        public int Amount { get; init; }
        public string Type { get; init; }
    }
}
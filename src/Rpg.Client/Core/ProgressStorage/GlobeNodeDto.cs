namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record GlobeNodeDto
    {
        public bool IsAvailable { get; init; }
        public GlobeNodeSid Sid { get; init; }
    }
}
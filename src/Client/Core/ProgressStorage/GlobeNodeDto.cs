using Client.Core;

namespace Client.Core.ProgressStorage;

internal sealed record GlobeNodeDto
{
    public bool IsAvailable { get; init; }
    public ILocationSid Sid { get; init; }
}
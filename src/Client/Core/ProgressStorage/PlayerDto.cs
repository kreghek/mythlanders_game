namespace Client.Core.ProgressStorage;

internal sealed record PlayerDto
{
    public string?[]? Abilities { get; init; }
    public GroupDto? Group { get; init; }
    public string?[]? KnownMonsterSids { get; init; }
    public GroupDto? Pool { get; init; }
    public ResourceDto?[]? Resources { get; init; }
}
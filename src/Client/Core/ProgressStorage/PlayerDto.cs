namespace Client.Core.ProgressStorage;

internal sealed record PlayerDto
{
    public string?[]? Abilities { get; init; }
    public string?[]? KnownMonsterSids { get; init; }
    public GroupDto? Heroes { get; init; }
    public ResourceDto?[]? Resources { get; init; }
}
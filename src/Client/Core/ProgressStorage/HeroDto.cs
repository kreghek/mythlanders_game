namespace Client.Core.ProgressStorage;

internal sealed record HeroDto
{
    public string? HeroSid { get; init; }
    public int Hp { get; init; }
}
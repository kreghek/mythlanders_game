namespace Client.Core.ProgressStorage;

internal sealed record PlayerUnitDto
{
    public string HeroSid { get; init; }
    public int Hp { get; init; }
}
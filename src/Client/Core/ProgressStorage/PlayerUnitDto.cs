namespace Client.Core.ProgressStorage;

internal sealed record PlayerUnitDto
{
    public int Hp { get; init; }

    public string HeroSid { get; init; }
}
﻿namespace Client.Core.ProgressStorage;

internal sealed record PlayerDto
{
    public string?[]? Abilities { get; init; }
    public string?[]? AvailableLocations { get; init; }
    public HeroDto?[]? Heroes { get; init; }
    public string?[]? KnownMonsterSids { get; init; }
    public string?[]? MonsterPerks { get; init; }
    public ResourceDto?[]? Resources { get; init; }
}
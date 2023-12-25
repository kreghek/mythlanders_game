namespace Core.Props;

public sealed record PropScheme(string Sid) : IPropScheme
{
    public string?[]? Tags { get; init; }
}
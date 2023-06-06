namespace Client.Core;

public record DamageResult
{
    public int? ValueFinal { get; init; }
    public int ValueSource { get; init; }
}
namespace Rpg.Client.Core
{
    public record DamageResult
    {
        public int ValueSource { get; init; }
        public int? ValueFinal { get; init; }
    }
}
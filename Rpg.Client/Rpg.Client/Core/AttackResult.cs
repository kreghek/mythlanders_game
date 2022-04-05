namespace Rpg.Client.Core
{
    public record DamageResult
    {
        public int? ValueFinal { get; init; }
        public int ValueSource { get; init; }
        public int? ValueToShield { get; init; }
    }
}
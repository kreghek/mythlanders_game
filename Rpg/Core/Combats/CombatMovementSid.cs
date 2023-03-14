namespace Core.Combats;

public sealed record CombatMovementSid(string Value)
{
    public static implicit operator CombatMovementSid(string source) => new(source);
    public static implicit operator string(CombatMovementSid source) => source.Value;

};

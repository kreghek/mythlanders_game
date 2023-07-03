namespace Core.Combats;

public record CombatantEffectSid(string Value) : ICombatantEffectSid, IComparable
{
    public int CompareTo(object? obj)
    {
        return Value.CompareTo(((CombatantEffectSid?)obj)?.Value);
    }

    public override string ToString()
    {
        return Value;
    }
}
namespace Core.Combats;

public sealed class CombatantEffectSid : ICombatantEffectSid
{
    private readonly string _value;

    public CombatantEffectSid(string value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value;
    }
}
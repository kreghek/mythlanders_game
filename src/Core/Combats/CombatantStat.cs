namespace Core.Combats;

public sealed class CombatantStat : IUnitStat
{
    public CombatantStat(UnitStatType type, IStatValue value)
    {
        Type = type;
        Value = value;
    }

    public UnitStatType Type { get; }
    public IStatValue Value { get; }
}
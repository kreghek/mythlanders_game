using CombatDicesTeam.Combats;

namespace Client.Core.Heroes;

public class UnitStat : IUnitStat
{
    public UnitStat(ICombatantStatType type)
    {
        Type = type;
        Value = new StatValue(0);
    }

    public UnitStat(ICombatantStatType type, int baseValue)
    {
        Type = type;
        Value = new StatValue(baseValue);
    }

    public ICombatantStatType Type { get; }

    public IStatValue Value { get; }
}
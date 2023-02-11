namespace Core.Combats;

public interface IUnitStat
{
    UnitStatType Type { get; }
    IStatValue Value { get; }
}
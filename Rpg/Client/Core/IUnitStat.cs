namespace Rpg.Client.Core
{
    internal interface IUnitStat
    {
        UnitStatType Type { get; }
        IStatValue Value { get; }
    }
}
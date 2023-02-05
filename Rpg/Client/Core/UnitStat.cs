namespace Rpg.Client.Core
{
    internal class UnitStat : IUnitStat
    {
        public UnitStat(UnitStatType type)
        {
            Type = type;
            Value = new StatValue(0);
        }

        public UnitStat(UnitStatType type, int baseValue)
        {
            Type = type;
            Value = new StatValue(baseValue);
        }

        public UnitStatType Type { get; }
        public IStatValue Value { get; }
    }
}
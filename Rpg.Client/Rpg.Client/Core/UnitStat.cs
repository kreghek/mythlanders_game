namespace Rpg.Client.Core
{
    internal class UnitStat
    {
        public UnitStat(UnitStatType type)
        {
            Type = type;
            Value = new Stat(0);
        }

        public UnitStat(UnitStatType type, int baseValue)
        {
            Type = type;
            Value = new Stat(baseValue);
        }

        public UnitStatType Type { get; }
        public Stat Value { get; }
    }
}
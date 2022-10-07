namespace Rpg.Client.Core
{
    internal class CombatUnitStat : IUnitStat
    {
        public CombatUnitStat(IUnitStat baseStat)
        {
            Type = baseStat.Type;
            Value = new CombatStatValue(baseStat.Value);
        }

        public UnitStatType Type { get; }

        public IStatValue Value { get; }
    }
}
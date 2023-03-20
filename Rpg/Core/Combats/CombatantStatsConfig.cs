namespace Core.Combats;

public sealed class CombatantStatsConfig
{
    private readonly IDictionary<UnitStatType, IStatValue> _stats;

    public CombatantStatsConfig()
    {
        _stats = new Dictionary<UnitStatType, IStatValue>
        {
            {
                UnitStatType.ShieldPoints, new CombatantStatValue(new StatValue(1))
            },
            {
                UnitStatType.HitPoints, new CombatantStatValue(new StatValue(3))
            },
            {
                UnitStatType.Resolve, new CombatantStatValue(new StatValue(8))
            },
            {
                UnitStatType.Maneuver, new CombatantStatValue(new StatValue(1))
            },
            {
                UnitStatType.Defense, new CombatantStatValue(new StatValue(0))
            }
        };
    }

    public IReadOnlyCollection<IUnitStat> GetStats()
    {
        return _stats.Select(x => new CombatantStat(x.Key, x.Value)).ToArray();
    }

    public void SetValue(UnitStatType statType, int value)
    {
        _stats[statType].ChangeBase(value);
    }
}
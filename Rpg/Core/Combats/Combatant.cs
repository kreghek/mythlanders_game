namespace Core.Combats;

public class Combatant
{
    private readonly IList<CombatMovement> _pool;
    private readonly CombatMovement[] _hand;

    public IReadOnlyCollection<IUnitStat> Stats { get; }

    public Combatant(CombatMovementSequence sequence)
    {
        _pool = new List<CombatMovement>();
        _hand = new CombatMovement[3];
        
        foreach (var combatMovement in sequence.Items)
        {
            _pool.Add(combatMovement);
        }

        Stats = new List<IUnitStat>()
        {
            new CombatantStat(UnitStatType.ShieldPoints, new CombatantStatValue(new StatValue(1))),
            new CombatantStat(UnitStatType.HitPoints, new CombatantStatValue(new StatValue(3))),
            new CombatantStat(UnitStatType.Resolve, new CombatantStatValue(new StatValue(8))),
            new CombatantStat(UnitStatType.Maneuver, new CombatantStatValue(new StatValue(1)))
        };
    }

    public string? Sid { get; set; }

    public IReadOnlyCollection<CombatMovement> Pool => _pool.ToArray();

    public IReadOnlyCollection<CombatMovement> Hand => _hand;

    public void StartCombat()
    {
        for (var i = 0; i < 3; i++)
        {
            if (_pool.Any())
            {
                _hand[i] = _pool.First();
                _pool.RemoveAt(0);
            }
        }
    }

    public bool IsDead { get; }

    public bool IsPlayerControlled { get; set; }
}
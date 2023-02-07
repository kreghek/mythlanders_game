namespace Core.Combats;

public class Combatant
{
    private IList<CombatMovement> _pool;
    private CombatMovement[] _hand;
    
    public Combatant(CombatMovementSequence sequence)
    {
        _pool = new List<CombatMovement>();
        _hand = new CombatMovement[3];
        
        foreach (var combatMovement in sequence.Items)
        {
            _pool.Add(combatMovement);
        }
    }

    public string? Sid { get; set; }

    public IReadOnlyCollection<CombatMovement> Pool => _pool.ToArray();

    public IReadOnlyCollection<CombatMovement> Hand => _hand;

    public void StartCombat()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_pool.Any())
            {
                _hand[i] = _pool.First();
                _pool.RemoveAt(0);
            }
        }
    }
}
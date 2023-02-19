namespace Core.Combats;

public class Combatant
{
    private readonly IList<ICombatantEffect> _effects = new List<ICombatantEffect>();
    private readonly CombatMovementInstance?[] _hand;
    private readonly IList<CombatMovementInstance> _pool;

    public Combatant(CombatMovementSequence sequence)
    {
        _pool = new List<CombatMovementInstance>();
        _hand = new CombatMovementInstance?[3];

        foreach (var combatMovement in sequence.Items)
        {
            var instance = new CombatMovementInstance(combatMovement);
            _pool.Add(instance);
        }

        Stats = new List<IUnitStat>
        {
            new CombatantStat(UnitStatType.ShieldPoints, new CombatantStatValue(new StatValue(1))),
            new CombatantStat(UnitStatType.HitPoints, new CombatantStatValue(new StatValue(3))),
            new CombatantStat(UnitStatType.Resolve, new CombatantStatValue(new StatValue(8))),
            new CombatantStat(UnitStatType.Maneuver, new CombatantStatValue(new StatValue(1))),
            new CombatantStat(UnitStatType.Defense, new CombatantStatValue(new StatValue(0)))
        };
    }

    public IReadOnlyCollection<ICombatantEffect> Effects => _effects.ToArray();

    public IReadOnlyList<CombatMovementInstance?> Hand => _hand;

    public CombatMovementInstance? PopNextPoolMovement()
    {
        var move = _pool.FirstOrDefault();
        if (move is not null)
        {
            _pool.RemoveAt(0);
        }

        return move;
    }

    public void AssignMoveToHand(int handIndex, CombatMovementInstance movement)
    {
        _hand[handIndex] = movement;
    }

    public bool IsDead { get; private set; }

    public bool IsPlayerControlled { get; init; }

    public string? Sid { get; init; }

    public IReadOnlyCollection<IUnitStat> Stats { get; }

    public void SetDead()
    {
        IsDead = true;
    }

    public void AddEffect(ICombatantEffect effect)
    {
        effect.Impose(this);
        _effects.Add(effect);
    }

    public void RemoveEffect(ICombatantEffect effect)
    {
        effect.Dispel(this);
        _effects.Remove(effect);
    }

    public void StartCombat()
    {
        for (var i = 0; i < 3; i++)
            if (_pool.Any())
            {
                _hand[i] = _pool.First();
                _pool.RemoveAt(0);
            }
    }

    public void UpdateEffects(CombatantEffectUpdateType updateType)
    {
        var effectToDispel = new List<ICombatantEffect>();
        foreach (var effect in _effects)
        {
            effect.Update(updateType);

            if (effect.Lifetime.IsDead) effectToDispel.Add(effect);
        }

        foreach (var effect in effectToDispel)
        {
            effect.Dispel(this);
            RemoveEffect(effect);
        }
    }

    internal void DropMovementFromHand(CombatMovementInstance movement)
    {
        for (var i = 0; i < _hand.Length; i++)
            if (_hand[i] == movement)
                _hand[i] = null;
    }
}
namespace Core.Combats;

public sealed class Combatant
{
    private readonly IList<ICombatantEffect> _effects = new List<ICombatantEffect>();
    private readonly CombatMovementInstance?[] _hand;
    private readonly IList<CombatMovementInstance> _pool;

    public Combatant(string classSid, CombatMovementSequence sequence, CombatantStatsConfig stats,
        ICombatActorBehaviour behaviour)
    {
        ClassSid = classSid;
        Behaviour = behaviour;
        _pool = new List<CombatMovementInstance>();
        _hand = new CombatMovementInstance?[3];

        foreach (var combatMovement in sequence.Items)
        {
            var instance = new CombatMovementInstance(combatMovement);
            _pool.Add(instance);
        }

        Stats = stats.GetStats();
    }

    public ICombatActorBehaviour Behaviour { get; }

    public string ClassSid { get; }

    public IReadOnlyCollection<ICombatantEffect> Effects => _effects.ToArray();

    public IReadOnlyList<CombatMovementInstance?> Hand => _hand;

    public bool IsDead { get; private set; }

    public bool IsPlayerControlled { get; init; }

    public string? Sid { get; init; }
    public IReadOnlyCollection<IUnitStat> Stats { get; }

    public void AddEffect(ICombatantEffect effect, ICombatantEffectLifetimeImposeContext context)
    {
        effect.Impose(this);
        _effects.Add(effect);
        
        effect.Lifetime.EffectImposed(effect, context);
    }

    public void AssignMoveToHand(int handIndex, CombatMovementInstance movement)
    {
        _hand[handIndex] = movement;
    }

    public CombatMovementInstance? PopNextPoolMovement()
    {
        var move = _pool.FirstOrDefault();
        if (move is not null) _pool.RemoveAt(0);

        return move;
    }

    public void PrepareToCombat()
    {
        for (var i = 0; i < 3; i++)
        {
            if (!_pool.Any())
                // Pool is empty.
                // Stop to prepare first movements.
                break;

            _hand[i] = _pool.First();
            _pool.RemoveAt(0);
        }
    }

    private void RemoveEffect(ICombatantEffect effect, ICombatantEffectLifetimeDispelContext context)
    {
        effect.Dispel(this);
        _effects.Remove(effect);
        
        effect.Lifetime.EffectDispelled(effect, context);
    }

    public void SetDead()
    {
        IsDead = true;
    }

    public void UpdateEffects(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeDispelContext effectLifetimeDispelContext)
    {
        var context = new CombatantEffectLifetimeUpdateContext(this);

        var effectToDispel = new List<ICombatantEffect>();
        foreach (var effect in _effects)
        {
            effect.Update(updateType, context);

            if (effect.Lifetime.IsDead) effectToDispel.Add(effect);
        }

        foreach (var effect in effectToDispel)
        {
            effect.Dispel(this);
            RemoveEffect(effect, effectLifetimeDispelContext);
        }
    }

    internal int? DropMovementFromHand(CombatMovementInstance movement)
    {
        for (var i = 0; i < _hand.Length; i++)
            if (_hand[i] == movement)
            {
                _hand[i] = null;
                return i;
            }

        return null;
    }
}
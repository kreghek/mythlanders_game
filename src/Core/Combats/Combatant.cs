using Core.Combats.CombatantEffects;

namespace Core.Combats;

public sealed class Combatant
{
    private readonly IList<ICombatantEffect> _effects = new List<ICombatantEffect>();
    private readonly CombatMovementInstance?[] _hand;
    private readonly IList<CombatMovementInstance> _pool;
    private readonly IReadOnlyCollection<ICombatantEffectFactory> _startupEffects;

    public Combatant(string classSid,
        CombatMovementSequence sequence,
        CombatantStatsConfig stats,
        ICombatActorBehaviour behaviour,
        IReadOnlyCollection<ICombatantEffectFactory> startupEffects)
    {
        _startupEffects = startupEffects;
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

    /// <summary>
    /// Activity source of the combatant. May be CPU-driven or wait player's intentions.
    /// </summary>
    public ICombatActorBehaviour Behaviour { get; }

    /// <summary>
    /// Identifier of class.
    /// Class is the group of combatant.
    /// </summary>
    public string ClassSid { get; }

    /// <summary>
    /// Identifier for debug.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? DebugSid { get; init; }

    /// <summary>
    /// Current combatant effects.
    /// </summary>
    public IReadOnlyCollection<ICombatantEffect> Effects => _effects.ToArray();

    /// <summary>
    /// Current available combatant's movements.
    /// </summary>
    public IReadOnlyList<CombatMovementInstance?> Hand => _hand;

    /// <summary>
    /// Is the combatant active?
    /// </summary>
    public bool IsDead { get; private set; }

    /// <summary>
    /// Combatant side. Player of CPU.
    /// </summary>
    public bool IsPlayerControlled { get; init; }

    /// <summary>
    /// Combatant's movements to whole combat.
    /// </summary>
    public IReadOnlyList<CombatMovementInstance> Pool => _pool.ToArray();

    /// <summary>
    /// Current combatant stats.
    /// </summary>
    public IReadOnlyCollection<IUnitStat> Stats { get; }

    /// <summary>
    /// Add effect to combatant.
    /// </summary>
    /// <param name="effect">Effect instance.</param>
    /// <param name="lifetimeImposeContext">
    /// Content to add effect. To handle some reaction on new effects (change stats, moves, other
    /// effects).
    /// </param>
    public void AddEffect(ICombatantEffect effect, ICombatantEffectImposeContext effectImposeContext,
        ICombatantEffectLifetimeImposeContext lifetimeImposeContext)
    {
        effect.Impose(this, effectImposeContext);
        _effects.Add(effect);

        effect.Lifetime.HandleOwnerImposed(effect, lifetimeImposeContext);
    }

    /// <summary>
    /// Assign move from pool to hand.
    /// </summary>
    /// <param name="handIndex">Index of hand slot.</param>
    /// <param name="movement">Combat movement instance.</param>
    public void AssignMoveToHand(int handIndex, CombatMovementInstance movement)
    {
        _hand[handIndex] = movement;
    }

    /// <summary>
    /// Extract movement from pool if it exists.
    /// </summary>
    /// <returns>Combat movement instance.</returns>
    public CombatMovementInstance? PopNextPoolMovement()
    {
        var move = _pool.FirstOrDefault();
        if (move is not null)
        {
            _pool.RemoveAt(0);
        }

        return move;
    }

    /// <summary>
    /// Initial method to make combatant ready to fight.
    /// </summary>
    /// <param name="combatCore"></param>
    public void PrepareToCombat(CombatCore combatCore)
    {
        StartupHand();
        ApplyStartupEffects(combatCore);
    }

    /// <summary>
    /// Deactivate combatant.
    /// He is not combatant yet.
    /// </summary>
    public void SetDead()
    {
        IsDead = true;
    }

    /// <summary>
    /// Update combatant effects.
    /// </summary>
    public void UpdateEffects(CombatantEffectUpdateType updateType,
        ICombatantEffectLifetimeDispelContext effectLifetimeDispelContext)
    {
        var context = new CombatantEffectLifetimeUpdateContext(this, effectLifetimeDispelContext.Combat);

        var effectToDispel = new List<ICombatantEffect>();
        foreach (var effect in _effects)
        {
            effect.Update(updateType, context);

            if (effect.Lifetime.IsExpired)
            {
                effectToDispel.Add(effect);
            }
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
        {
            if (_hand[i] == movement)
            {
                _hand[i] = null;
                return i;
            }
        }

        return null;
    }

    private void ApplyStartupEffects(CombatCore combatCore)
    {
        foreach (var effectFactory in _startupEffects)
        {
            var effect = effectFactory.Create();

            var effectImposeContext = new CombatantEffectImposeContext(combatCore);

            var effectLifetimeImposeContext = new CombatantEffectLifetimeImposeContext(this, combatCore);

            AddEffect(effect, effectImposeContext, effectLifetimeImposeContext);
        }
    }

    public void RemoveEffect(ICombatantEffect effect, ICombatantEffectLifetimeDispelContext context)
    {
        effect.Dispel(this);
        _effects.Remove(effect);

        effect.Lifetime.HandleOwnerDispelled(effect, context);
    }

    private void StartupHand()
    {
        for (var i = 0; i < 3; i++)
        {
            var combatMove = PopNextPoolMovement();
            if (combatMove is null)
            // Pool is empty.
            // Stop to prepare first movements.
            {
                break;
            }

            _hand[i] = combatMove;
        }
    }
}
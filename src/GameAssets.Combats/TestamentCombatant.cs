using Core.Combats;
using Core.Combats.CombatantStatuses;

namespace GameAssets.Combats;

public sealed class TestamentCombatant : ICombatant
{
    private readonly CombatMovementInstance?[] _hand;
    private readonly IList<CombatMovementInstance> _pool;
    private readonly IReadOnlyCollection<ICombatantStatusFactory> _startupStatuses;
    private readonly IList<ICombatantStatus> _statuses = new List<ICombatantStatus>();

    public TestamentCombatant(string classSid,
        CombatMovementSequence sequence,
        CombatantStatsConfig stats,
        ICombatActorBehaviour behaviour,
        IReadOnlyCollection<ICombatantStatusFactory> startupStatuses)
    {
        _startupStatuses = startupStatuses;
        ClassSid = classSid;
        Behaviour = behaviour;

        _combatMoveContainers = new Dictionary<ICombatMovementContainerType, ICombatMovementContainer>
        {
            { CombatMovementContainerTypes.Hand, new CombatMovementContainer(CombatMovementContainerTypes.Hand) },
            { CombatMovementContainerTypes.Pool, new CombatMovementContainer(CombatMovementContainerTypes.Pool) },
        };
        
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
    /// Current combatant effects.
    /// </summary>
    public IReadOnlyCollection<ICombatantStatus> Statuses => _statuses.ToArray();

    /// <summary>
    /// Add a status to combatant.
    /// </summary>
    public void AddStatus(ICombatantStatus effect, ICombatantStatusImposeContext statusImposeContext,
        ICombatantStatusLifetimeImposeContext lifetimeImposeContext)
    {
        effect.Impose(this, statusImposeContext);
        _statuses.Add(effect);

        effect.Lifetime.HandleImposed(effect, lifetimeImposeContext);
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
    public void PrepareToCombat(ICombatantStartupContext context)
    {
        StartupHand();
        ApplyStartupEffects(context);
    }

    public void RemoveStatus(ICombatantStatus effect, ICombatantStatusLifetimeDispelContext context)
    {
        effect.Dispel(this);
        _statuses.Remove(effect);

        effect.Lifetime.HandleDispelling(effect, context);
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
    public void UpdateStatuses(CombatantStatusUpdateType updateType,
        ICombatantStatusLifetimeDispelContext effectLifetimeDispelContext)
    {
        var context = new CombatantEffectLifetimeUpdateContext(this, effectLifetimeDispelContext.Combat);

        var statusesToDispel = new List<ICombatantStatus>();
        foreach (var status in _statuses)
        {
            status.Update(updateType, context);

            if (status.Lifetime.IsExpired)
            {
                statusesToDispel.Add(status);
            }
        }

        foreach (var effect in statusesToDispel)
        {
            effect.Dispel(this);
            RemoveStatus(effect, effectLifetimeDispelContext);
        }
    }

    public IReadOnlyCollection<ICombatMovementContainer> CombatMovementContainers => _combatMoveContainers.Values.ToArray();

    private readonly IDictionary<ICombatMovementContainerType, ICombatMovementContainer> _combatMoveContainers;

    public ICombatMovementContainer GetCombatMovementContainer(ICombatMovementContainerType containerType)
    {
        return _combatMoveContainers[containerType];
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

    private void ApplyStartupEffects(ICombatantStartupContext context)
    {
        foreach (var effectFactory in _startupStatuses)
        {
            var effect = effectFactory.Create();

            var effectImposeContext = context.ImposeStatusContext;

            var effectLifetimeImposeContext = context.ImposeStatusLifetimeContext;

            AddStatus(effect, effectImposeContext, effectLifetimeImposeContext);
        }
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
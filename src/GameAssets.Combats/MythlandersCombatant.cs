using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats.CombatantStatusSources;

namespace GameAssets.Combats;

public sealed class MythlandersCombatant : ICombatant
{
    private readonly IDictionary<ICombatMovementContainerType, ICombatMovementContainer> _combatMoveContainers;
    private readonly IReadOnlyCollection<ICombatantStatusFactory> _startupStatuses;
    private readonly IList<ICombatantStatus> _statuses;

    public MythlandersCombatant(string classSid,
        CombatMovementSequence sequence,
        CombatantStatsConfig stats,
        ICombatActorBehaviour behaviour,
        IReadOnlyCollection<ICombatantStatusFactory> startupStatuses)
    {
        _startupStatuses = startupStatuses;
        ClassSid = classSid;
        Behaviour = behaviour;

        _statuses = new List<ICombatantStatus>();

        _combatMoveContainers = new Dictionary<ICombatMovementContainerType, ICombatMovementContainer>
        {
            { CombatMovementContainerTypes.Hand, new CombatMovementContainer(CombatMovementContainerTypes.Hand) },
            { CombatMovementContainerTypes.Pool, new CombatMovementContainer(CombatMovementContainerTypes.Pool) }
        };

        var hand = GetCombatMovementContainer(CombatMovementContainerTypes.Hand);
        for (var i = 0; i < 3; i++)
        {
            hand.AppendMove(null);
        }

        foreach (var combatMovement in sequence.Items)
        {
            var instance = new CombatMovementInstance(combatMovement);
            _combatMoveContainers[CombatMovementContainerTypes.Pool].AppendMove(instance);
        }

        Stats = stats.GetStats();
    }

    /// <summary>
    /// Assign move from pool to hand.
    /// </summary>
    /// <param name="handIndex">Index of hand slot.</param>
    /// <param name="movement">Combat movement instance.</param>
    public void AssignMoveToHand(int handIndex, CombatMovementInstance movement)
    {
        GetMovementContainer(CombatMovementContainerTypes.Hand).SetMove(movement, handIndex);
    }

    public ICombatMovementContainer GetCombatMovementContainer(ICombatMovementContainerType containerType)
    {
        return _combatMoveContainers[containerType];
    }

    /// <summary>
    /// Extract movement from pool if it exists.
    /// </summary>
    /// <returns>Combat movement instance.</returns>
    public CombatMovementInstance? PopNextPoolMovement()
    {
        var pool = GetMovementContainer(CombatMovementContainerTypes.Pool);
        var move = pool.GetItems().FirstOrDefault();
        if (move is not null)
        {
            pool.RemoveAt(0);
        }

        return move;
    }

    internal int? DropMovementFromHand(CombatMovementInstance movement)
    {
        var hand = GetCombatMovementContainer(CombatMovementContainerTypes.Hand);
        for (var i = 0; i < hand.GetItems().Count; i++)
        {
            if (hand.GetItems()[i] == movement)
            {
                hand.SetMove(null, i);
                return i;
            }
        }

        return null;
    }

    private void ApplyStartupEffects(ICombatantStartupContext context)
    {
        foreach (var effectFactory in _startupStatuses)
        {
            var effect = effectFactory.Create(new StartupStatusSource());

            var effectImposeContext = context.ImposeStatusContext;

            var effectLifetimeImposeContext = context.ImposeStatusLifetimeContext;

            AddStatus(effect, effectImposeContext, effectLifetimeImposeContext);
        }
    }

    private ICombatMovementContainer GetMovementContainer(ICombatMovementContainerType containerType)
    {
        return CombatMovementContainers.Single(container => container.Type == containerType);
    }

    private void StartupHand()
    {
        var hand = GetCombatMovementContainer(CombatMovementContainerTypes.Hand);

        for (var i = 0; i < 3; i++)
        {
            var combatMove = PopNextPoolMovement();
            if (combatMove is null)
                // Pool is empty.
                // Stop to prepare first movements.
            {
                break;
            }

            hand.SetMove(combatMove, i);
        }
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
    /// Is the combatant active?
    /// </summary>
    public bool IsDead { get; private set; }

    /// <summary>
    /// Combatant side. Player of CPU.
    /// </summary>
    public bool IsPlayerControlled { get; init; }

    /// <summary>
    /// Current combatant stats.
    /// </summary>
    public IReadOnlyCollection<ICombatantStat> Stats { get; }

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
    /// Initial method to make combatant ready to fight.
    /// </summary>
    /// <param name="context"></param>
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
        var context = new CombatantStatusLifetimeUpdateContext(this, effectLifetimeDispelContext.Combat);

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

    public IReadOnlyCollection<ICombatMovementContainer> CombatMovementContainers =>
        _combatMoveContainers.Values.ToArray();
}
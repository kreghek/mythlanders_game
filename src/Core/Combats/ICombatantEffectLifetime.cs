namespace Core.Combats;

/// <summary>
/// Combatant effect lifetime to control effect must be dispelled.
/// </summary>
public interface ICombatantEffectLifetime
{
    /// <summary>
    /// Effect is expired and must be dispelled.
    /// </summary>
    bool IsExpired { get; }

    /// <summary>
    /// Do work to release lifetime object then effect-owner was dispelled.
    /// </summary>
    /// <param name="owner">Effect owns lifetime object.</param>
    /// <param name="context"> Context object to access to combat environment. </param>
    /// <remarks>
    /// Example:
    /// Used to monitor combatant activities. 
    /// </remarks>
    void HandleOwnerDispelled(ICombatantEffect owner, ICombatantEffectLifetimeDispelContext context);

    /// <summary>
    /// Do work to prepare lifetime object then effect-owner was imposed.
    /// </summary>
    /// <param name="owner"> Effect owns lifetime object. </param>
    /// <param name="context"> Context object to access to combat environment. </param>
    /// <remarks>
    /// Example:
    /// Used to monitor combatant activities. 
    /// </remarks>
    void HandleOwnerImposed(ICombatantEffect owner, ICombatantEffectLifetimeImposeContext context);

    /// <summary>
    /// Updates your own state according combat life cycle.
    /// </summary>
    /// <param name="updateType"> Current combat life cycle. </param>
    /// <param name="context"> Context object to access to combat environment. </param>
    void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context);
}
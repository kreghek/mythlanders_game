namespace Client.GameScreens.Combat;

/// <summary>
/// Current combat shade state.
/// </summary>
/// <remarks>
/// Used to detect combat movement visualization and shade environment.
/// </remarks>
internal interface ICombatShadeContext 
{
    /// <summary>
    /// Current scope is defined when any actors in the focus.
    /// </summary>
    ICombatShadeScope? CurrentScope { get; }
}

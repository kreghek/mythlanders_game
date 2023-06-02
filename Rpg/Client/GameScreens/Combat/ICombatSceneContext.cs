namespace Client.GameScreens.Combat;

/// <summary>
/// Current combat context.
/// </summary>
/// <remarks>
/// Used to detect combat movement visualization and shade environment.
/// </remarks>
internal interface ICombatSceneContext 
{
    /// <summary>
    /// Current scope is defined when any actors in the focus.
    /// </summary>
    ICombatSceneScope? CurrentScope { get; }
}

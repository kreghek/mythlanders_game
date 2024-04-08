using System.Collections.Generic;

using Client.Engine;

using CombatDicesTeam.Combats;

namespace Client.Assets.CombatMovements;

/// <summary>
/// Service to provide visualization data of the combat movement.
/// </summary>
internal interface ICombatMovementVisualizationProvider
{
    /// <summary>
    /// Extract actual key values of combat movement to display in on UI.
    /// </summary>
    /// <param name="combatMovementInstance">Combat movement from which a values should be extracted.</param>
    /// <returns>Set of values or empty.</returns>
    IReadOnlyList<DescriptionKeyValue> ExtractCombatMovementValues(
        CombatMovementInstance combatMovementInstance);

    /// <summary>
    /// Gets Combat movement icon.
    /// </summary>
    /// <param name="sid">Combat movement identifier.</param>
    /// <returns>Icon data.</returns>
    CombatMovementIcon GetMoveIcon(CombatMovementSid sid);

    /// <summary>
    /// Gets actor'c movement scene data to vidualize movement.
    /// </summary>
    /// <param name="sid">Combat movement identifier.</param>
    /// <param name="actorAnimator">Actors animator to animate.</param>
    /// <param name="movementExecution">Data of movement execution.</param>
    /// <param name="visualizationContext">Context of combat movement.</param>
    /// <returns>Returns combat movement scene.</returns>
    CombatMovementScene GetMovementVisualizationState(CombatMovementSid sid, IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}
using System.Collections.Generic;

using Client.Engine;

using CombatDicesTeam.Combats;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementFactory
{
    CombatMovementIcon CombatMovementIcon { get; }
    string Sid { get; }
    CombatMovement CreateMovement();

    CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);

    /// <summary>
    /// Extracts a actual key values of CM to display on UI.
    /// </summary>
    /// <param name="combatMovementInstance">Combat movement.</param>
    /// <returns>Returns set of values with meta-data or empty array.</returns>
    IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(CombatMovementInstance combatMovementInstance);
}

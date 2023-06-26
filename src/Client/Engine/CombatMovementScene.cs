using System.Collections.Generic;

using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;

namespace Client.Engine;

/// <summary>
/// Desription to visualize combat movement scene.
/// </summary>
/// <param name="ActorState">Actor movements root state.</param>
/// <param name="CameraOperatorTaskSequence"> Sequence of the camera operator tasks in order to perform. </param>
/// <remarks>
/// Combinations of:
/// - Actor movements.
/// - Camera Operator tasks.
/// Potentially:
/// - Post-processing effects.
/// - Sound operator tasks.
/// </remarks>
internal record CombatMovementScene(
    IActorVisualizationState ActorState,
    IReadOnlyList<ICameraOperatorTask> CameraOperatorTaskSequence);
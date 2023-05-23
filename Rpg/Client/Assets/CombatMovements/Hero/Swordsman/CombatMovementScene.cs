using System.Collections.Generic;

using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal record CombatMovementScene(IActorVisualizationState ActorState, IReadOnlyList<ICameraOperatorTask> CameraOperatorTaskSequence);
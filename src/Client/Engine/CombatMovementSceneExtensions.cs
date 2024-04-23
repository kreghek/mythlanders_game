using System.Linq;

using Client.GameScreens.Combat.GameObjects.CommonStates;

namespace Client.Engine;

internal static class CombatMovementSceneExtensions
{
    public static CombatMovementScene MergeWith(this CombatMovementScene first, CombatMovementScene second)
    {
        return new CombatMovementScene(new SequentialState(first.ActorState, second.ActorState),
            first.CameraOperatorTaskSequence.Union(second.CameraOperatorTaskSequence).ToArray());
    }
}
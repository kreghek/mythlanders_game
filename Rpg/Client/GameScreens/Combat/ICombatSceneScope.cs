using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

internal interface ICombatSceneScope
{
    IReadOnlyList<IActorAnimator> Actors { get; }
}

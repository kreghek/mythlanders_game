using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

/// <summary>
/// Current combat scope. Store actors in the focus.
/// </summary>
internal interface ICombatShadeScope
{
    IReadOnlyList<IActorAnimator> FocusedActors { get; }
}
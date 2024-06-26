﻿using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

/// <summary>
/// Base combat scope implementation.
/// </summary>
internal sealed class CombatShadeScope : ICombatShadeScope
{
    public CombatShadeScope(IReadOnlyList<IActorAnimator> actors)
    {
        FocusedActors = actors;
    }

    public IReadOnlyList<IActorAnimator> FocusedActors { get; }
}
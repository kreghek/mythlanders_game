using System;
using System.Collections.Generic;

using CombatDicesTeam.Combats;

namespace Client.Core;

public class NullGameProgressionTransition : IGameProgressionTransition
{
    public IEnumerable<GameProgressionTrigger> Triggers => ArraySegment<GameProgressionTrigger>.Empty;
    public IEnumerable<GameProgressionEntry> Entries => ArraySegment<GameProgressionEntry>.Empty;
    public IGameProgressionTransition Next => Singleton<NullGameProgressionTransition>.Instance;
}
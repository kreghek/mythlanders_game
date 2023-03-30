using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core;

internal sealed class CombatSequence
{
    public CombatSequence()
    {
        CompletedCombats = new List<CombatSource>();
        Combats = Array.Empty<CombatSource>();
    }

    public IReadOnlyList<CombatSource> Combats { get; set; }

    public IList<CombatSource> CompletedCombats { get; }
}
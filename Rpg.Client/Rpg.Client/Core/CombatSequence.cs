using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class CombatSequence
    {
        public CombatSequence()
        {
            CompletedCombats = new List<Combat>();
            Combats = Array.Empty<Combat>();
        }

        public IReadOnlyCollection<Combat> Combats { get; set; }

        public IList<Combat> CompletedCombats { get; set; }
    }
}
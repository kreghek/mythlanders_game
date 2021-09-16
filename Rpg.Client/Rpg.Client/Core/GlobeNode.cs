using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class GlobeNode
    {
        public GlobeNode(string name)
        {
            Name = name;
        }

        public Event? AvailableDialog { get; set; }

        public CombatSequence? CombatSequence { get; set; }

        public int Index { get; internal set; }

        public string Name { get; }
    }

    internal sealed class CombatSequence
    {
        public CombatSequence()
        {
            CompletedCombats = new List<Combat>();
            Combats = Array.Empty<Combat>();
        }

        public IList<Combat> CompletedCombats { get; set; }
        public IReadOnlyCollection<Combat> Combats { get; set; }
    }
}
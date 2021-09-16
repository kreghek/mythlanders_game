using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal class GlobeNode
    {
        public GlobeNode(string name)
        {
            Name = name;

            Combats = Array.Empty<Combat>();
        }

        public Event? AvailableDialog { get; set; }
        public IReadOnlyCollection<Combat> Combats { get; set; }

        public int Index { get; internal set; }

        public string Name { get; }
    }
}
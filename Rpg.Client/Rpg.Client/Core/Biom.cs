using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Biom
    {
        public string Name { get; set; }

        public IEnumerable<GlobeNode> Nodes { get; set; }

        public bool IsAvailable { get; set; }

        public int Level { get; set; }

        public bool IsComplete { get; set; }
        public string UnlockBiom { get; internal set; }
    }
}

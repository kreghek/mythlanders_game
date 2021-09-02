using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Biom
    {
        public bool IsAvailable { get; set; }

        public bool IsComplete { get; set; }

        public int Level { get; set; }
        public string Name { get; set; }

        public IEnumerable<GlobeNode> Nodes { get; set; }
        public string UnlockBiom { get; internal set; }
    }
}
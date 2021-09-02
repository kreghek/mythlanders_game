using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Biom
    {
        public bool IsAvailable { get; set; }

        public bool IsComplete { get; set; }

        public int Level { get; set; }
        public BiomType Type { get; set; }

        public IEnumerable<GlobeNode> Nodes { get; set; }

        public BiomType? UnlockBiom { get; internal set; }

        public bool IsStartBiom { get; set; }
        public bool IsFinalBiom { get; set; }
    }
}
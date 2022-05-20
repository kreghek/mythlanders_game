using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class PoolGroup
    {
        public PoolGroup()
        {
            Units = Array.Empty<Unit>();
        }

        public IEnumerable<Unit> Units { get; set; }
    }
}
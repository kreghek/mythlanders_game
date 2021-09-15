using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Group
    {
        public Group()
        {
            Units = Array.Empty<Unit>();
        }

        public IEnumerable<Unit> Units { get; set; }
    }
}
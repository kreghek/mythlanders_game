using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal class Group
    { 
        public IEnumerable<Unit> Units { get; set; }
    }

    internal class Globe
    {
        public Group PlayerGroup { get; set; }
    }
}

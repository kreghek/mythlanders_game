using System.Collections.Generic;

namespace Rpg.Client.Core
{
    public sealed class Dialog
    {
        public int Counter { get; set; }
        public bool IsUnique { get; set; }
        public IEnumerable<DialogNode> Nodes { get; init; }
        public DialogNode StartNode { get; init; }
    }
}
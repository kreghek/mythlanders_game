using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Dialog
    {
        public DialogNode StartNode { get; init; }
        public IEnumerable<DialogNode> Nodes { get; init; }
        public bool IsUnique { get; set; }
        public int Counter { get; set; }
    }
}
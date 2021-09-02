using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class DialogNode
    {
        public IEnumerable<DialogOption> Options { get; set; }
        public string Text { get; init; }
    }
}
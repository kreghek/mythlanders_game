using System.Collections.Generic;

namespace Rpg.Client.Core
{
    public sealed class DialogNode
    {
        public IEnumerable<DialogOption> Options { get; set; }
        public string Text { get; init; }
    }
}
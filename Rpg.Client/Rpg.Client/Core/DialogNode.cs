using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class DialogNode
    {
        public string Text { get; init; }

        public IEnumerable<DialogOption> Options { get; set; }
    }
}
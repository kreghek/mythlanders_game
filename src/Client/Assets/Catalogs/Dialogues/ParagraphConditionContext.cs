using System.Collections.Generic;
using System.Linq;

using Client.Core;

namespace Client.Assets.Catalogs.Dialogues;

internal class ParagraphConditionContext
{
    public ParagraphConditionContext(Player player)
    {
        CurrentHeroes = player.Heroes.Select(x => x.ClassSid).ToArray();
    }
    
    /// <inheritdoc />
    public IReadOnlyCollection<string> CurrentHeroes { get; }
}
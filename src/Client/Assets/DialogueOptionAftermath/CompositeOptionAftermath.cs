using System.Collections.Generic;
using System.Linq;

using Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class CompositeOptionAftermath : IDialogueOptionAftermath
{
    private readonly IDialogueOptionAftermath[] _list;

    public CompositeOptionAftermath(IEnumerable<IDialogueOptionAftermath> list)
    {
        _list = list.ToArray();
    }

    public void Apply(IEventContext dialogContext)
    {
        foreach (var item in _list)
        {
            item.Apply(dialogContext);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class CompositeOptionAftermath : IDialogueOptionAftermath<AftermathContext>
{
    private readonly IDialogueOptionAftermath<AftermathContext>[] _list;

    public CompositeOptionAftermath(IEnumerable<IDialogueOptionAftermath<AftermathContext>> list)
    {
        _list = list.ToArray();
    }

    public void Apply(AftermathContext aftermathContext)
    {
        foreach (var item in _list)
        {
            item.Apply(aftermathContext);
        }
    }
}
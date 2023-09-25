using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class CompositeOptionAftermath : IDialogueOptionAftermath<AftermathContext>
{
    private readonly IDialogueOptionAftermath<AftermathContext>[] _list;

    public bool IsHidden => !_list.Any(x => !x.IsHidden);

    public CompositeOptionAftermath(IEnumerable<IDialogueOptionAftermath<AftermathContext>> list)
    {
        _list = list.ToArray();
    }

    public string GetDescription(AftermathContext aftermathContext)
    {
        return string.Join(Environment.NewLine, _list.Select(x => x.GetDescription(aftermathContext)));
    }

    public void Apply(AftermathContext aftermathContext)
    {
        foreach (var item in _list)
        {
            item.Apply(aftermathContext);
        }
    }
}
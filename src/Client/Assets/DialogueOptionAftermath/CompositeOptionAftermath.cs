using System;
using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class CompositeOptionAftermath<TAftermathContext> : IDialogueOptionAftermath<TAftermathContext>
{
    private readonly IDialogueOptionAftermath<TAftermathContext>[] _list;

    public CompositeOptionAftermath(IEnumerable<IDialogueOptionAftermath<TAftermathContext>> list)
    {
        _list = list.ToArray();
    }

    public bool IsHidden => _list.All(x => x.IsHidden);

    public string GetDescription(TAftermathContext aftermathContext)
    {
        return string.Join(Environment.NewLine, _list.Select(x => x.GetDescription(aftermathContext)));
    }

    public void Apply(TAftermathContext aftermathContext)
    {
        foreach (var item in _list)
        {
            item.Apply(aftermathContext);
        }
    }
}
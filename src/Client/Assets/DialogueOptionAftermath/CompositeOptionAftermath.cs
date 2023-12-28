using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class CompositeOptionAftermath : IDialogueOptionAftermath<CampaignAftermathContext>
{
    private readonly IDialogueOptionAftermath<CampaignAftermathContext>[] _list;

    public CompositeOptionAftermath(IEnumerable<IDialogueOptionAftermath<CampaignAftermathContext>> list)
    {
        _list = list.ToArray();
    }

    public bool IsHidden => !_list.Any(x => !x.IsHidden);

    public string GetDescription(CampaignAftermathContext aftermathContext)
    {
        return string.Join(Environment.NewLine, _list.Select(x => x.GetDescription(aftermathContext)));
    }

    public void Apply(CampaignAftermathContext aftermathContext)
    {
        foreach (var item in _list)
        {
            item.Apply(aftermathContext);
        }
    }
}
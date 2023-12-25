using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class PredefinedCombatOptionAftermath : DialogueOptionAftermathBase
{
    private readonly string _sid;

    public PredefinedCombatOptionAftermath(string sid)
    {
        _sid = sid;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.StartCombat(_sid);
    }

    protected override IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return Array.Empty<string>();
    }
}
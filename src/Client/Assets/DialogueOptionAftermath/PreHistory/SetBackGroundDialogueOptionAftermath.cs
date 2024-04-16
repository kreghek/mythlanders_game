using System;
using System.Collections.Generic;

using Client.Assets.DialogueOptionAftermath.Campaign;
using Client.GameScreens.PreHistory;

namespace Client.Assets.DialogueOptionAftermath.PreHistory;

internal class SetBackGroundDialogueOptionAftermath : PreHistoryDialogueOptionAftermathBase,
    IDecorativeEnvironmentAftermath<PreHistoryAftermathContext>
{
    private readonly string _backgroundName;

    public SetBackGroundDialogueOptionAftermath(string backgroundName)
    {
        _backgroundName = backgroundName;

        IsHidden = true;
    }

    protected override IReadOnlyList<object> GetDescriptionValues(PreHistoryAftermathContext aftermathContext)
    {
        return Array.Empty<object>();
    }

    public override void Apply(PreHistoryAftermathContext aftermathContext)
    {
        aftermathContext.SetBackground(_backgroundName);
    }
}
using System;
using System.Collections.Generic;

using Client.GameScreens.PreHistory;

namespace Client.Assets.DialogueOptionAftermath.PreHistory;

internal sealed class PlaySongDialogueOptionAftermath : PreHistoryDialogueOptionAftermathBase,
    IDecorativeEnvironmentAftermath<PreHistoryAftermathContext>
{
    private readonly string _resourceName;

    public PlaySongDialogueOptionAftermath(string resourceName)
    {
        _resourceName = resourceName;

        IsHidden = true;
    }

    protected override IReadOnlyList<string> GetDescriptionValues(PreHistoryAftermathContext aftermathContext)
    {
        return ArraySegment<string>.Empty;
    }

    public override void Apply(PreHistoryAftermathContext aftermathContext)
    {
        aftermathContext.PlaySong(_resourceName);
    }
}
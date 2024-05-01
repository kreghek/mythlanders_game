using System;
using System.Collections.Generic;

using Client.GameScreens.PreHistory;

namespace Client.Assets.DialogueOptionAftermath.PreHistory;

internal sealed class PlayEffectDialogueOptionAftermath : PreHistoryDialogueOptionAftermathBase,
    IDecorativeEnvironmentAftermath<PreHistoryAftermathContext>
{
    private readonly string _effectSid;
    private readonly string _resourceName;

    public PlayEffectDialogueOptionAftermath(string effectSid, string resourceName)
    {
        _effectSid = effectSid;
        _resourceName = resourceName;

        IsHidden = true;
    }

    protected override IReadOnlyList<string> GetDescriptionValues(PreHistoryAftermathContext aftermathContext)
    {
        return ArraySegment<string>.Empty;
    }

    public override void Apply(PreHistoryAftermathContext aftermathContext)
    {
        aftermathContext.PlaySoundEffect(_effectSid, _resourceName);
    }
}
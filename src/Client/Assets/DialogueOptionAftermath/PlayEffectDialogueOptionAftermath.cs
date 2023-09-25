using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class PlayEffectDialogueOptionAftermath :DialogueOptionAftermathBase, IDecorativeEnvironmentAftermath
{
    private readonly string _effectSid;
    private readonly string _resourceName;

    public PlayEffectDialogueOptionAftermath(string effectSid, string resourceName)
    {
        _effectSid = effectSid;
        _resourceName = resourceName;

        IsHidden = true;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.PlaySoundEffect(_effectSid, _resourceName);
    }

    protected override IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return ArraySegment<string>.Empty;
    }
}
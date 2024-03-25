using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class PlayEffectDialogueOptionAftermath : CampaignDialogueOptionAftermathBase, IDecorativeEnvironmentAftermath<CampaignAftermathContext>
{
    private readonly string _effectSid;
    private readonly string _resourceName;

    public PlayEffectDialogueOptionAftermath(string effectSid, string resourceName)
    {
        _effectSid = effectSid;
        _resourceName = resourceName;

        IsHidden = true;
    }

    protected override IReadOnlyList<string> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return ArraySegment<string>.Empty;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        aftermathContext.PlaySoundEffect(_effectSid, _resourceName);
    }
}
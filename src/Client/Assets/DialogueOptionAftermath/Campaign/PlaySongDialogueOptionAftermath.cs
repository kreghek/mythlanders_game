using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class PlaySongDialogueOptionAftermath : CampaignDialogueOptionAftermathBase, IDecorativeEnvironmentAftermath<CampaignAftermathContext>
{
    private readonly string _resourceName;

    public PlaySongDialogueOptionAftermath(string resourceName)
    {
        _resourceName = resourceName;

        IsHidden = true;
    }

    protected override IReadOnlyList<string> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return ArraySegment<string>.Empty;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        aftermathContext.PlaySong(_resourceName);
    }
}
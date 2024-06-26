﻿using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal class ActivateStoryPointOptionAftermath : CampaignDialogueOptionAftermathBase
{
    private readonly string _storyPointSid;

    public ActivateStoryPointOptionAftermath(string storyPointSid)
    {
        _storyPointSid = storyPointSid;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        aftermathContext.AddStoryPoint(_storyPointSid);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return new[] { _storyPointSid };
    }
}
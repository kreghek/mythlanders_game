﻿using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class AddHeroOptionAftermath : CampaignDialogueOptionAftermathBase
{
    private readonly string _heroSid;

    public AddHeroOptionAftermath(string heroSid)
    {
        _heroSid = heroSid;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        aftermathContext.AddNewHero(_heroSid);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return new object[]
        {
            _heroSid
        };
    }
}
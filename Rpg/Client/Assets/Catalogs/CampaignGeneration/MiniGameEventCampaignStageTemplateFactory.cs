using System;
using System.Collections.Generic;

using Client.Assets.StageItems;

using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class MiniGameEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly CampaignStageTemplateServices _services;

    public MiniGameEventCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var roll = _services.Dice.Roll(3);
        return roll switch
        {
            1 => new SlidingPuzzlesMiniGameStageItem(),
            2 => new TowersMinigameStageItem(),
            3 => new Match3MinigameStageItem(),
            _ => throw new InvalidOperationException()
        };
    }
}
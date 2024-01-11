using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.CampaignEffects;
using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

using CombatDicesTeam.GenericRanges;

using Core.PropDrop;

namespace Client.Assets.StageItems;

internal sealed class RewardStageItem : IRewardCampaignStageItem
{
    private readonly IDropResolver _dropResolver;

    public RewardStageItem(IDropResolver dropResolver)
    {
        _dropResolver = dropResolver;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var campaignResources = CreateCampaignResources(currentCampaign.Location);
        var drop = _dropResolver.Resolve(campaignResources);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign,
                drop.Select(x => new ResourceCampaignEffect(x)).ToArray()));
    }


    public IReadOnlyCollection<ICampaignEffect> GetEstimateRewards(HeroCampaignLocation heroCampaign)
    {
        var campaignResources = CreateCampaignResources(heroCampaign);

        var drop = _dropResolver.Resolve(campaignResources);

        return drop.Select(x => new ResourceCampaignEffect(x)).ToArray();
    }
}
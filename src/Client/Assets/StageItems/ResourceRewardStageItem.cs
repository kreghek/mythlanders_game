using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.CampaignRewards;
using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

using CombatDicesTeam.GenericRanges;

using Core.PropDrop;

namespace Client.Assets.StageItems;

internal sealed class ResourceRewardStageItem : IRewardCampaignStageItem
{
    private readonly IDropResolver _dropResolver;
    private readonly GlobeProvider _globeProvider;
    private readonly IJobProgressResolver _jobProgressResolver;

    public ResourceRewardStageItem(GlobeProvider globeProvider,
        IJobProgressResolver jobProgressResolver, IDropResolver dropResolver)
    {
        _globeProvider = globeProvider;
        _jobProgressResolver = jobProgressResolver;
        _dropResolver = dropResolver;
    }

    private static IReadOnlyCollection<IDropTableScheme> CreateCampaignResources(HeroCampaignLocation currentCampaign)
    {
        static IReadOnlyCollection<IDropTableScheme> GetLocationResourceDrop(string sid)
        {
            return new[]
            {
                new DropTableScheme(sid, new IDropTableRecordSubScheme[]
                {
                    new DropTableRecordSubScheme(null, GenericRange<int>.CreateMono(1), sid, 1)
                }, 1)
            };
        }

        switch (currentCampaign.Sid.ToString())
        {
            case nameof(LocationSids.Thicket):
                return GetLocationResourceDrop("snow");

            case nameof(LocationSids.Desert):
                return GetLocationResourceDrop("sand");
        }

        return ArraySegment<IDropTableScheme>.Empty;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var completeCampaignProgress = new CampaignCompleteJobProgress();
        var currentJobs = _globeProvider.Globe.ActiveStoryPoints.ToArray();

        foreach (var job in currentJobs)
        {
            _jobProgressResolver.ApplyProgress(completeCampaignProgress, job);
        }

        var campaignResources = CreateCampaignResources(currentCampaign.Location);
        var drop = _dropResolver.Resolve(campaignResources);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign,
                drop.Select(x => new ResourceCampaignReward(x)).ToArray()));
    }


    public IReadOnlyCollection<ICampaignReward> GetEstimateRewards(HeroCampaignLocation heroCampaign)
    {
        var campaignResources = CreateCampaignResources(heroCampaign);

        var drop = _dropResolver.Resolve(campaignResources);

        return drop.Select(x => new ResourceCampaignReward(x)).ToArray();
    }
}
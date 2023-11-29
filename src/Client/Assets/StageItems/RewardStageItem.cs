﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

using CombatDicesTeam.GenericRanges;

using Core.PropDrop;

namespace Client.Assets.StageItems;

internal class RewardStageItem : ICampaignStageItem
{
    private readonly IDropResolver _dropResolver;
    private readonly GlobeProvider _globeProvider;
    private readonly IJobProgressResolver _jobProgressResolver;

    public RewardStageItem(GlobeProvider globeProvider,
        IJobProgressResolver jobProgressResolver, IDropResolver dropResolver)
    {
        _globeProvider = globeProvider;
        _jobProgressResolver = jobProgressResolver;
        _dropResolver = dropResolver;
    }

    private static IReadOnlyCollection<IDropTableScheme> CreateCampaignResources(HeroCampaign currentCampaign)
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

        switch (currentCampaign.Location.ToString())
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

        var campaignResources = CreateCampaignResources(currentCampaign);
        var drop = _dropResolver.Resolve(campaignResources);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign, drop));
    }
}
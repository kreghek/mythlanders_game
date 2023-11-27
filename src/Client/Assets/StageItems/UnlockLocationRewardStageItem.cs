using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

using Core.PropDrop;

namespace Client.Assets.StageItems;

internal sealed class UnlockLocationRewardStageItem : IRewardCampaignStageItem
{
    private readonly GlobeProvider _globeProvider;
    private readonly ILocationSid _scoutedLocation;
    
    private readonly IDropResolver _dropResolver;
    private readonly IJobProgressResolver _jobProgressResolver;

    public UnlockLocationRewardStageItem(GlobeProvider globeProvider,
        IJobProgressResolver jobProgressResolver, IDropResolver dropResolver,
        ILocationSid scoutedLocation)
    {
        _globeProvider = globeProvider;
        _scoutedLocation = scoutedLocation;
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

        _globeProvider.Globe.CurrentAvailableLocations.Add(_scoutedLocation);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign, drop));
    }
}
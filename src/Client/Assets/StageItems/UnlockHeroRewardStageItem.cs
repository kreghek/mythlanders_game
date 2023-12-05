using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.CampaignRewards;
using Client.Core.Campaigns;
using Client.Core.Heroes;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

using CombatDicesTeam.Combats;

namespace Client.Assets.StageItems;

internal sealed class UnlockHeroRewardStageItem : IRewardCampaignStageItem
{
    private readonly GlobeProvider _globeProvider;
    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly UnitName _jointedHeroName;
    private readonly IUnitSchemeCatalog _unitSchemeCatalog;

    public UnlockHeroRewardStageItem(GlobeProvider globeProvider,
        IJobProgressResolver jobProgressResolver,
        UnitName jointedHeroName,
        IUnitSchemeCatalog unitSchemeCatalog)
    {
        _globeProvider = globeProvider;
        _jobProgressResolver = jobProgressResolver;
        _jointedHeroName = jointedHeroName;
        _unitSchemeCatalog = unitSchemeCatalog;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var completeCampaignProgress = new CampaignCompleteJobProgress();
        var currentJobs = _globeProvider.Globe.ActiveStoryPoints.ToArray();

        foreach (var job in currentJobs)
        {
            _jobProgressResolver.ApplyProgress(completeCampaignProgress, job);
        }

        var freeSlots = Enumerable.Range(0, 6)
            .Except(_globeProvider.Globe.Player.Heroes.Select(x =>
                x.FormationPosition.ColumentIndex * 2 + x.FormationPosition.LineIndex)).ToArray();

        if (freeSlots.Any())
        {
            var firstFreeSlot = freeSlots.First();
            _globeProvider.Globe.Player.AddHero(new HeroState(_jointedHeroName.ToString().ToLowerInvariant(),
                new StatValue(3), new FieldCoords(firstFreeSlot / 2, firstFreeSlot % 2)));
        }
        else
        {
            var hero = new Hero(_unitSchemeCatalog.Heroes[_jointedHeroName], 1);
            _globeProvider.Globe.Player.Pool.AddNewUnit(hero);
        }

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign, GetEstimateRewards(currentCampaign)));
    }

    public IReadOnlyCollection<ICampaignReward> GetEstimateRewards(HeroCampaign heroCampaign)
    {
        return new[] { new HeroCampaignReward(_jointedHeroName) };
    }
}
using System.Linq;

using Client.Core;
using Client.Core.CampaignEffects;
using Client.Core.Campaigns;
using Client.GameScreens.Campaign;
using Client.GameScreens.CommandCenter;
using Client.ScreenManagement;

using CombatDicesTeam.Combats;

namespace Client.Engine;

internal class StateCoordinator
{
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly GlobeProvider _globeProvider;
    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly ScenarioCampaigns _scenarioCampaigns;
    private readonly IScreenManager _screenManager;

    public StateCoordinator(GlobeProvider globeProvider, IScreenManager screenManager,
        ICampaignGenerator campaignGenerator, ScenarioCampaigns scenarioCampaigns,
        IJobProgressResolver jobProgressResolver)
    {
        _globeProvider = globeProvider;
        _screenManager = screenManager;
        _campaignGenerator = campaignGenerator;
        _scenarioCampaigns = scenarioCampaigns;
        _jobProgressResolver = jobProgressResolver;
    }

    public void MakeCombatFailureTransition(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        var globe = _globeProvider.Globe;

        if (globe.Features.HasFeature(GameFeatures.Campaigns))
        {
            ResetCampaign(currentScreen);
        }
        else
        {
            AutoRetryCampaignStage(currentScreen, currentCampaign);
        }
    }

    public void MakeCombatWinTransition(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        var globe = _globeProvider.Globe;

        if (globe.Features.HasFeature(GameFeatures.CampaignMap))
        {
            _screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.Campaign,
                new CampaignScreenTransitionArguments(currentCampaign));
        }
        else
        {
            AutoSelectNextCampaignStage(currentScreen, currentCampaign);
        }
    }

    public void MakeCommonTransition(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        var globe = _globeProvider.Globe;

        if (globe.Features.HasFeature(GameFeatures.CampaignMap))
        {
            _screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.Campaign,
                new CampaignScreenTransitionArguments(currentCampaign));
        }
        else
        {
            AutoSelectNextCampaignStage(currentScreen, currentCampaign);
        }
    }

    public void MakeGoalStageTransition(IScreen currentScreen, HeroCampaign currentCampaign, Globe globe)
    {
        if (currentCampaign.ActualRewards.OfType<CompleteDemoCampaignEffect>().Any())
        {
            ShowDemoScreen(currentScreen);
        }
        else
        {
            currentCampaign.WinCampaign(globe, _jobProgressResolver);
            ResetCampaign(currentScreen);
        }
    }

    public void MakeStartTransition(IScreen currentScreen)
    {
        var campaign = _scenarioCampaigns.GetCampaign("tutorial", _globeProvider.Globe.Player);

        var startNode = campaign.Location.Stages.GetAllNodes().First();
        var startStage = startNode.Payload;

        campaign.CurrentStage = startNode;

        startStage.ExecuteTransition(currentScreen, _screenManager, campaign);
    }

    private void AutoRetryCampaignStage(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        foreach (var hero in currentCampaign.Heroes)
        {
            hero.HitPoints.Restore(hero.HitPoints.ActualMax);
        }

        currentCampaign.CurrentStage.Payload.ExecuteTransition(currentScreen, _screenManager, currentCampaign);
    }

    private void AutoSelectNextCampaignStage(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        var nextStage = currentCampaign.Location.Stages.GetNext(currentCampaign.CurrentStage).First();

        currentCampaign.CurrentStage = nextStage;

        nextStage.Payload.ExecuteTransition(currentScreen, _screenManager, currentCampaign);
    }

    private void ResetCampaign(IScreen currentScreen)
    {
        var campaigns = _campaignGenerator.CreateSet(_globeProvider.Globe);

        _screenManager.ExecuteTransition(
            currentScreen,
            ScreenTransition.CommandCenter,
            new CommandCenterScreenTransitionArguments(campaigns));
    }

    private void ShowDemoScreen(IScreen currentScreen)
    {
        _screenManager.ExecuteTransition(
            currentScreen,
            ScreenTransition.Demo,
            null!);
    }
}
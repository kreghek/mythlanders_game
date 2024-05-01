using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Campaign;
using Client.ScreenManagement;

namespace Client.Engine;

internal class StateCoordinator
{
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly GlobeProvider _globeProvider;
    private readonly ScenarioCampaigns _scenarioCampaigns;
    private readonly IScreenManager _screenManager;

    public StateCoordinator(GlobeProvider globeProvider, IScreenManager screenManager,
        ICampaignGenerator campaignGenerator, ScenarioCampaigns scenarioCampaigns)
    {
        _globeProvider = globeProvider;
        _screenManager = screenManager;
        _campaignGenerator = campaignGenerator;
        _scenarioCampaigns = scenarioCampaigns;
    }

    public void MakeCombatFailureTransition(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        var globe = _globeProvider.Globe;

        if (globe.Features.HasFeature(GameFeatures.Campaigns))
        {
            _screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.Campaign,
                new CampaignScreenTransitionArguments(currentCampaign));
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
        currentCampaign.CurrentStage.Payload.ExecuteTransition(currentScreen, _screenManager, currentCampaign);
    }

    private void AutoSelectNextCampaignStage(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        var nextStage = currentCampaign.Location.Stages.GetNext(currentCampaign.CurrentStage).First();

        currentCampaign.CurrentStage = nextStage;

        nextStage.Payload.ExecuteTransition(currentScreen, _screenManager, currentCampaign);
    }
}
using System;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Campaign;
using Client.GameScreens.Combat;
using Client.GameScreens.CommandCenter;
using Client.ScreenManagement;

namespace Client.Engine;
internal class StateCoordinator
{
    private readonly GlobeProvider _globeProvider;
    private readonly IScreenManager _screenManager;
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly ScenarioCampaigns _scenarioCampaigns;

    public StateCoordinator(GlobeProvider globeProvider, IScreenManager screenManager, ICampaignGenerator campaignGenerator, ScenarioCampaigns scenarioCampaigns)
    {
        _globeProvider = globeProvider;
        _screenManager = screenManager;
        _campaignGenerator = campaignGenerator;
        _scenarioCampaigns = scenarioCampaigns;
    }

    public void MakeStartTransition(IScreen currentScreen)
    {
        MoveToScreen(currentScreen, _globeProvider.Globe);
    }
    
    public void MakeCombatWinTransition(IScreen currentScreen, HeroCampaign currentCampaign)
    {
        var globe = _globeProvider.Globe;
        
        if (globe.Progression.HasEntry("CampaignMapAvailable"))
        {
            _screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.Campaign,
                new CampaignScreenTransitionArguments(currentCampaign));
        }
        else
        {
            if (globe.Progression.HasEntry("TutorialComplete"))
            {
                throw new NotImplementedException();
            }
            else
            {
                var campaign = _scenarioCampaigns.GetCampaign("tutorial", globe.Player);

                var startStage = campaign.CurrentStage.Payload;
                
                startStage.ExecuteTransition(currentScreen, _screenManager, currentCampaign);
            }
        }
    }
    
    public void MakeCombatFailureTransition(IScreen currentScreen)
    {
        var globe = _globeProvider.Globe;
        
        if (globe.Progression.HasEntry("CommandCenterAvailable"))
        {
            var availableLaunches = _campaignGenerator.CreateSet(globe);

            _screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.CommandCenter,
                new CommandCenterScreenTransitionArguments(availableLaunches));
        }
        else
        {
            if (globe.Progression.HasEntry("TutorialComplete"))
            {
                var campaigns = _campaignGenerator.CreateSet(_globeProvider.Globe);

                _screenManager.ExecuteTransition(currentScreen, ScreenTransition.CommandCenter,
                    new CommandCenterScreenTransitionArguments(campaigns));
            }
            else
            {
                var campaign = _scenarioCampaigns.GetCampaign("tutorial", globe.Player);

                var startStage = campaign.Location.Stages.GetAllNodes().First().Payload;

                _screenManager.ExecuteTransition(
                    currentScreen,
                    ScreenTransition.Combat,
                    new CombatScreenTransitionArguments(campaign,
                        ((CombatStageItem)startStage).CombatSequence, 0, false, campaign.Location.Sid,
                        null));
            }
        }
    }

    private void MoveToScreen(IScreen currentScreen, Globe globe)
    {
        if (globe.Progression.HasEntry("CommandCenterAvailable"))
        {
            var availableLaunches = _campaignGenerator.CreateSet(globe);

            _screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.CommandCenter,
                new CommandCenterScreenTransitionArguments(availableLaunches));
        }
        else
        {
            var campaign = _scenarioCampaigns.GetCampaign("tutorial", globe.Player);

            var startStage = campaign.Location.Stages.GetAllNodes().First().Payload;

            _screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.Combat,
                new CombatScreenTransitionArguments(campaign,
                    ((CombatStageItem)startStage).CombatSequence, 0, false, campaign.Location.Sid,
                    null));
        }
    }
}

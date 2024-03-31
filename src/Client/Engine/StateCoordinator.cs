using System;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Campaign;
using Client.GameScreens.Combat;
using Client.GameScreens.CommandCenter;
using Client.GameScreens.TextDialogue;
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
                var nextStage = currentCampaign.Location.Stages.GetNext(currentCampaign.CurrentStage).First();

                currentCampaign.CurrentStage = nextStage;

                nextStage.Payload.ExecuteTransition(currentScreen, _screenManager, currentCampaign);
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
                var tutorialCampaign = _scenarioCampaigns.GetCampaign("tutorial", globe.Player);

                var startStage = tutorialCampaign.Location.Stages.GetAllNodes().First().Payload;

                startStage.ExecuteTransition(currentScreen, _screenManager, tutorialCampaign);
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

            var startNode = campaign.Location.Stages.GetAllNodes().First();
            var startStage = startNode.Payload;

            campaign.CurrentStage = startNode;

            startStage.ExecuteTransition(currentScreen, _screenManager, campaign);

            //if (startStage is CombatStageItem combatStage)
            //{
            //    startStage.ExecuteTransition(currentScreen, _screenManager, campaign);

            //    //_screenManager.ExecuteTransition(
            //    //    currentScreen,
            //    //    ScreenTransition.Combat,
            //    //    new CombatScreenTransitionArguments(campaign,
            //    //        combatStage.CombatSequence, 0, false, campaign.Location.Sid,
            //    //        null));
            //}
            //else if (startStage is DialogueEventStageItem dialogueStage)
            //{
            //    dialogueStage.ExecuteTransition(currentScreen, _screenManager, campaign)

            //    _screenManager.ExecuteTransition(
            //        currentScreen,
            //        ScreenTransition.Event,
            //        new TextDialogueScreenTransitionArgs(campaign, dialogueStage.
            //            combatStage.CombatSequence, 0, false, campaign.Location.Sid,
            //            null));
            //}
        }
    }

    public void MakeCommonTransition(IScreen currentScreen, HeroCampaign currentCampaign)
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
                var nextStage = currentCampaign.Location.Stages.GetNext(currentCampaign.CurrentStage).First();

                currentCampaign.CurrentStage = nextStage;

                nextStage.Payload.ExecuteTransition(currentScreen, _screenManager, currentCampaign);
            }
        }
    }
}

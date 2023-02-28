using System.Resources;

using Client;
using Client.Assets.Catalogs;
using Client.Assets.Dialogues;

using Core.Dices;

using FluentAssertions;

using Rpg.Client.Assets;
using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.GameScreens.Speech.Ui;

namespace Content.SideQuests.Tests;

[TestFixture]
public class SynthAsParentTests
{
    [Test]
    public void CanonTest()
    {
        // Services
        var balanceTable = new BalanceTable();
        var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable, false);

        var eventCatalog = new DialogueCatalog(new DialogueResourceProvider(),
            new DialogueOptionAftermathCreator(unitSchemeCatalog));
        eventCatalog.Init();

        var storyPointCatalog = new StoryPointCatalog(eventCatalog);
        
        var globeProvider = new GlobeProvider(new LinearDice(), unitSchemeCatalog, eventCatalog,
            storyPointCatalog);
        
        globeProvider.GenerateNew();
        
        storyPointCatalog.Init(globeProvider.Globe);

        // Start testing

        var textEvent = eventCatalog.Events.Single(x => x.Sid == DialogueConstants.SideQuests.SynthAsParent.Sid);

        // Stage 1 - availability in a campaigns
        
        var requirements = textEvent.GetRequirements();

        var dialogueRequirementsContext =
            new DialogueEventRequirementContext(globeProvider.Globe, LocationSid.Desert, eventCatalog);

        var stage1IsAvailable = requirements.All(x => x.IsApplicableFor(dialogueRequirementsContext));

        stage1IsAvailable.Should().BeTrue();
        
        // Stage 1 - play dialogue and select canon

        var stage1Dialogue = eventCatalog.GetDialogue(textEvent.GetDialogSid());

        var stage1CanonDialoguePlayer = new DialoguePlayer(stage1Dialogue,
            new DialogueContextFactory(globeProvider.Globe, storyPointCatalog, globeProvider.Globe.Player, textEvent));

        var stage1TargetOptions = new[] { 1, 1, 1, 1, 1, 1, 3, 1 };
        foreach (var optionIndex in stage1TargetOptions)
        {
            foreach (var currentTextFragment in stage1CanonDialoguePlayer.CurrentTextFragments)
            {
                var (_, isLocalized) =
                    SpeechVisualizationHelper.PrepareLocalizedText(currentTextFragment.TextSid);

                isLocalized.Should().BeTrue();
            }
            
            foreach (var dualogueOption in stage1CanonDialoguePlayer.CurrentOptions)
            {
                var (_, isLocalized) =
                    SpeechVisualizationHelper.PrepareLocalizedText(dualogueOption.TextSid);

                isLocalized.Should().BeTrue();
            }
            
            var currentOptionList = stage1CanonDialoguePlayer.CurrentOptions.ToArray();
            var targetOption = currentOptionList[optionIndex - 1];
            stage1CanonDialoguePlayer.SelectOption(targetOption);
        }

        stage1CanonDialoguePlayer.IsEnd.Should().BeTrue();

        globeProvider.Globe.ActiveStoryPoints.Single().Sid.Should()
            .Be($"{DialogueConstants.SideQuests.SynthAsParent.Sid}_stage_1");
        
        // Quest is not available until in progress
        
        var stage1InProgressRequirements = textEvent.GetRequirements();
        var questIsNotAvailable = stage1InProgressRequirements.All(x => x.IsApplicableFor(dialogueRequirementsContext));
        questIsNotAvailable.Should().BeFalse();
        
        // Complete progress
        
        textEvent.Trigger(DialogueConstants.CompleteCurrentStageChallengeTrigger);
        
        // Quest available to continue

        var stage2Requirements = textEvent.GetRequirements();
        var questStage2IsAvailable = stage2Requirements.All(x => x.IsApplicableFor(dialogueRequirementsContext));

        questStage2IsAvailable.Should().BeTrue();
    }
}
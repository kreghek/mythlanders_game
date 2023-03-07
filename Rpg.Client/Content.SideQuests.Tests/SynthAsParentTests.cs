using System.Globalization;

using Client;
using Client.Assets.Catalogs;
using Client.Assets.Dialogues;
using Client.Core.Dialogues;

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
        var newCulture = CultureInfo.GetCultureInfo("ru-RU");
        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;

        var balanceTable = new BalanceTable();
        var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable, false);

        var eventCatalog = new DialogueCatalog(new LocalDialogueResourceProvider(),
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

        var stage1TargetOptions = new[] { 3, 1 };
        var stage1Dialogue = eventCatalog.GetDialogue(textEvent.GetDialogSid());
        CheckDialogue(stage1Dialogue, stage1TargetOptions, storyPointCatalog, globeProvider, textEvent);

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

        // Stage 2 - play dialogue to the end

        var stage2TargetOptions = new[] { 1, 1, 1 };
        var stage2Dialogue = eventCatalog.GetDialogue(textEvent.GetDialogSid());
        CheckDialogue(stage2Dialogue, stage2TargetOptions, storyPointCatalog, globeProvider, textEvent);

        globeProvider.Globe.ActiveStoryPoints
            .Single(x => x.Sid == $"{DialogueConstants.SideQuests.SynthAsParent.Sid}_stage_2").IsComplete.Should()
            .BeFalse();
    }

    private static void CheckDialogue(Dialogue testDialog, int[] targetOptions, StoryPointCatalog storyPointCatalog,
        GlobeProvider globeProvider, DialogueEvent textEvent)
    {
        var dialogueContextFactory = new DialogueContextFactory(globeProvider.Globe, storyPointCatalog,
            globeProvider.Globe.Player, textEvent);
        var dialoguePlayer = new DialoguePlayer(testDialog, dialogueContextFactory);

        foreach (var optionIndex in targetOptions)
        {
            foreach (var currentTextFragment in dialoguePlayer.CurrentTextFragments)
            {
                var (text, isLocalized) =
                    SpeechVisualizationHelper.PrepareLocalizedText(currentTextFragment.TextSid);

                text.Should().NotBeNullOrWhiteSpace();
                isLocalized.Should().BeTrue();
            }

            foreach (var dualogueOption in dialoguePlayer.CurrentOptions)
            {
                var (text, isLocalized) =
                    SpeechVisualizationHelper.PrepareLocalizedText(dualogueOption.TextSid);

                text.Should().NotBeNullOrWhiteSpace();
                isLocalized.Should().BeTrue();
            }

            var currentOptionList = dialoguePlayer.CurrentOptions.ToArray();
            var targetOption = currentOptionList[optionIndex - 1];
            dialoguePlayer.SelectOption(targetOption);
        }

        dialoguePlayer.IsEnd.Should().BeTrue();
    }
}
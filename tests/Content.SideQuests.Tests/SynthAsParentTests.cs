using System.Globalization;
using System.Reflection;

using Client;
using Client.Assets;
using Client.Assets.Catalogs;
using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Assets.Dialogues;
using Client.Assets.MonsterPerks;
using Client.Core;
using Client.Core.CampaignEffects;
using Client.Core.Campaigns;
using Client.GameScreens.TextDialogue.Ui;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;

using FluentAssertions;

using Moq;

namespace Content.SideQuests.Tests;

[TestFixture]
public class SynthAsParentTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var newCulture = CultureInfo.GetCultureInfo("ru-RU");
        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;
    }

    private static IReadOnlyCollection<TObj> GetAllLocationsFromStaticCatalog<TObj>(IReflect catalog)
    {
        return catalog
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(TObj))
            .Select(f => f.GetValue(null))
            .Where(v => v is not null)
            .Select(v => (TObj)v!)
            .ToArray();
    }

    [Test]
    public void CanonTest()
    {
        // Services

        var dice = new LinearDice();

        var balanceTable = new BalanceTable();
        var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable);

        var eventCatalog = new DialogueCatalog(new LocalDialogueResourceProvider(),
            new DialogueAftermathCreator(dice), new DialogueAftermathCreator(dice),
            new ParagraphConditionCreator());
        eventCatalog.Init();

        var storyPointCatalog = new StoryPointCatalog(eventCatalog);

        var monsterPerkCatalog = new MonsterPerkCatalog();

        var globeProvider = new GlobeProvider(unitSchemeCatalog,
            storyPointCatalog,
            monsterPerkCatalog);

        globeProvider.GenerateNew();

        storyPointCatalog.Init(globeProvider.Globe);

        // Start testing

        var textEvent = eventCatalog.Events.Single(x => x.Sid == DialogueConstants.SideQuests.SynthAsParent.Sid);

        QuestNotAvailableInOtherLocations(eventCatalog, globeProvider, textEvent, new[] { LocationSids.Desert });

        // Stage 1 - availability in a campaigns

        var requirements = textEvent.GetRequirements();

        var dialogueRequirementsContext =
            new DialogueEventRequirementContext(globeProvider.Globe, LocationSids.Desert, eventCatalog);

        var stage1IsAvailable = requirements.All(x => x.IsApplicableFor(dialogueRequirementsContext));

        stage1IsAvailable.Should().BeTrue();

        // Stage 1 - play dialogue and select canon

        var stage1TargetOptions = new[] { 3, 1 };
        var stage1Dialogue = eventCatalog.GetDialogue(textEvent.GetDialogSid());
        CheckDialogue(stage1Dialogue, stage1TargetOptions, storyPointCatalog, globeProvider, textEvent);

        globeProvider.Globe.ActiveStoryPoints.Single().Sid.Should()
            .Be($"{DialogueConstants.SideQuests.SynthAsParent.Sid}_stage_1");

        // Quest is not available until in progress

        CheckEventIsNotAvailableUntilInProgress(textEvent, dialogueRequirementsContext);

        // Complete progress

        textEvent.Trigger(DialogueConstants.CompleteCurrentStageChallengeTrigger);

        // Quest available to continue

        QuestNotAvailableInOtherLocations(eventCatalog, globeProvider, textEvent, new[] { LocationSids.Desert });

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

        // Quest is not available until in progress

        CheckEventIsNotAvailableUntilInProgress(textEvent, dialogueRequirementsContext);
    }

    private static void QuestNotAvailableInOtherLocations(DialogueCatalog eventCatalog, GlobeProvider globeProvider,
        DialogueEvent textEvent, ILocationSid[] availableLocations)
    {
        var allLocations = GetAllLocationsFromStaticCatalog<ILocationSid>(typeof(LocationSids));

        var notAvailableLocations = allLocations.Except(availableLocations).ToArray();

        foreach (var locationSid in notAvailableLocations)
        {
            var notAvailableLocationContext =
                new DialogueEventRequirementContext(globeProvider.Globe, locationSid, eventCatalog);

            var requirements1 = textEvent.GetRequirements();

            var stage1IsAvailable1 = requirements1.All(x => x.IsApplicableFor(notAvailableLocationContext));

            stage1IsAvailable1.Should().BeFalse();
        }
    }

    private static void CheckEventIsNotAvailableUntilInProgress(DialogueEvent textEvent,
        DialogueEventRequirementContext context)
    {
        var requirements = textEvent.GetRequirements();
        var questAvailability = requirements.All(x => x.IsApplicableFor(context));
        questAvailability.Should().BeFalse();
    }

    private static void CheckDialogue(Dialogue<ParagraphConditionContext, CampaignAftermathContext> testDialog,
        int[] targetOptions, StoryPointCatalog storyPointCatalog,
        GlobeProvider globeProvider, DialogueEvent textEvent)
    {
        var dialogueContextFactory = new DialogueContextFactory(globeProvider.Globe, storyPointCatalog,
            globeProvider.Globe.Player,
            Mock.Of<IDialogueEnvironmentManager>(),
            textEvent,
            new HeroCampaign(ArraySegment<(HeroState, FieldCoords)>.Empty,
                new HeroCampaignLocation(Mock.Of<ILocationSid>(), new DirectedGraph<ICampaignStageItem>()),
                ArraySegment<ICampaignEffect>.Empty, ArraySegment<ICampaignEffect>.Empty, default),
            Mock.Of<IEventContext>());
        var dialoguePlayer =
            new DialoguePlayer<ParagraphConditionContext, CampaignAftermathContext>(testDialog, dialogueContextFactory);

        foreach (var optionIndex in targetOptions)
        {
            foreach (var currentTextFragment in dialoguePlayer.CurrentTextFragments)
            {
                var (text, isLocalized) =
                    SpeechVisualizationHelper.PrepareLocalizedText(currentTextFragment.TextSid);

                text.Should().NotBeNullOrWhiteSpace();
                isLocalized.Should().BeTrue();
            }

            foreach (var dialogueOption in dialoguePlayer.CurrentOptions)
            {
                var (text, isLocalized) =
                    SpeechVisualizationHelper.PrepareLocalizedText(dialogueOption.TextSid);

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
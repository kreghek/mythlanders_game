using System.Globalization;
using System.Reflection;

using Client;
using Client.Assets;
using Client.Assets.Catalogs;
using Client.Assets.Dialogues;
using Client.Core;
using Client.Core.Dialogues;
using Client.GameScreens.TextDialogue;
using Client.GameScreens.TextDialogue.Ui;

using Core.Dices;

using FluentAssertions;

namespace Content.SideQuests.Tests;

[TestFixture]
public class MonkeyKingTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var newCulture = CultureInfo.GetCultureInfo("ru-RU");
        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;
    }

    [Test]
    public void CanonTest()
    {
        // Services

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

        var questLocations = new[] { LocationSids.Monastery };

        // Start testing

        var textEvent = eventCatalog.Events.Single(x => x.Sid == DialogueConstants.SideQuests.MonkeyKing.Sid);

        QuestNotAvailableInOtherLocations(eventCatalog, globeProvider, textEvent, questLocations);

        // Stage 1 - availability in a campaigns

        QuestAvailableInApplicableLocations(eventCatalog, globeProvider, textEvent, questLocations);

        // Stage 1 - play dialogue and select canon

        var stage1TargetOptions = new[] { 1, 1 };
        var stage1Dialogue = eventCatalog.GetDialogue(textEvent.GetDialogSid());
        CheckDialogue(stage1Dialogue, stage1TargetOptions, storyPointCatalog, globeProvider, textEvent);

        globeProvider.Globe.ActiveStoryPoints.Single().Sid.Should()
            .Be($"{DialogueConstants.SideQuests.MonkeyKing.Sid}_stage_1");

        // Quest is not available until in progress

        CheckEventIsNotAvailableUntilInProgress(eventCatalog, globeProvider, textEvent, questLocations);
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

    private static IReadOnlyCollection<TObj> GetAllLocationsFromStaticCatalog<TObj>(Type catalog)
    {
        return catalog
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(TObj))
            .Select(f => f.GetValue(null))
            .Where(v => v is not null)
            .Select(v => (TObj)v!)
            .ToArray();
    }

    private static void QuestAvailableInApplicableLocations(DialogueCatalog eventCatalog, GlobeProvider globeProvider,
        DialogueEvent textEvent, ILocationSid[] availableLocations)
    {
        foreach (var locationSid in availableLocations)
        {
            var notAvailableLocationContext =
                new DialogueEventRequirementContext(globeProvider.Globe, locationSid, eventCatalog);

            var requirements1 = textEvent.GetRequirements();

            var stage1IsAvailable1 = requirements1.All(x => x.IsApplicableFor(notAvailableLocationContext));

            stage1IsAvailable1.Should().BeTrue();
        }
    }

    private static void CheckEventIsNotAvailableUntilInProgress(DialogueCatalog eventCatalog,
        GlobeProvider globeProvider, DialogueEvent textEvent, ILocationSid[] availableLocations)
    {
        foreach (var locationSid in availableLocations)
        {
            var context = new DialogueEventRequirementContext(globeProvider.Globe, locationSid, eventCatalog);

            var requirements = textEvent.GetRequirements();
            var questAvailability = requirements.All(x => x.IsApplicableFor(context));
            questAvailability.Should().BeFalse();
        }
    }

    private static void CheckDialogue(Dialogue testDialog, int[] targetOptions, StoryPointCatalog storyPointCatalog,
        GlobeProvider globeProvider, DialogueEvent textEvent)
    {
        var dialogueContextFactory = new DialogueContextFactory(globeProvider.Globe, storyPointCatalog,
            globeProvider.Globe.Player, textEvent);
        var dialoguePlayer = new DialoguePlayer(testDialog, dialogueContextFactory);

        var targetOptionsIncludeFinish = targetOptions.Concat(new[] { 1 }).ToArray();

        foreach (var optionIndex in targetOptionsIncludeFinish)
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
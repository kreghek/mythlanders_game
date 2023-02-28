using System;
using System.Linq;
using System.Resources;

using Client.Assets.Catalogs;
using Client.Core;
using Client.Core.Dialogues;

using Core.Dices;

using Rpg.Client.Assets;
using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Speech;

namespace DialoguePlayer;

internal static class Program
{
    private static void Main()
    {
        var unitSchemeCatalog = new UnitSchemeCatalog(new BalanceTable(), isDemo: false);

        var resourceProvider = new DialogueResourceProvider();

        var aftermathCreator = new DialogueOptionAftermathCreator(unitSchemeCatalog);

        var eventCatalog = new DialogueCatalog(resourceProvider, aftermathCreator);
        eventCatalog.Init();

        var availableEvents = eventCatalog.Events;
        foreach (var dialogueEvent in availableEvents)
        {
            Console.WriteLine($@"- {dialogueEvent.Sid}");
        }

        var eventSid = Console.ReadLine();

        var selectedEvent = eventCatalog.Events.Single(x => x.Sid == eventSid);

        Console.WriteLine(@"Prepare event state:");
        while (true)
        {
            var triggerInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(triggerInput))
            {
                break;
            }

            var trigger = new DialogueEventTrigger(triggerInput);

            selectedEvent.Trigger(trigger);
        }

        var dialogueSid = selectedEvent.GetDialogSid();

        Console.WriteLine(@$"Selected dialogue sid: {dialogueSid}");

        var dialogue = eventCatalog.GetDialogue(dialogueSid);

        var storyPointCatalog = new StoryPointCatalog(eventCatalog);
        var dice = new LinearDice();
        var globeProvider = new GlobeProvider(dice, unitSchemeCatalog, eventCatalog, storyPointCatalog);
        globeProvider.GenerateNew();
        var globe = globeProvider.Globe;
        var player = new Player();

        var dialogPlayer = new Rpg.Client.Core.Dialogues.DialoguePlayer(dialogue,
            new DialogueContextFactory(globe, storyPointCatalog, player, selectedEvent));

        var assembly = dialogue.GetType().Assembly;
        var rm = new ResourceManager("Client.DialogueResources", assembly);

        while (!dialogPlayer.IsEnd)
        {
            foreach (var textFragment in dialogPlayer.CurrentTextFragments)
            {
                var speakerLocalized = GameObjectHelper.GetLocalized(textFragment.Speaker);
                var localizedSpeech = rm.GetString(textFragment.TextSid);
                Console.WriteLine($@"{speakerLocalized}: {localizedSpeech}");
            }

            var optionList = dialogPlayer.CurrentOptions.ToArray();
            for (var index = 0; index < optionList.Length; index++)
            {
                var option = optionList[index];
                var localizedOptionText = rm.GetString(option.TextSid);
                Console.WriteLine($@"{(index + 1)}. {localizedOptionText}");
            }

            // ReSharper disable once LocalizableElement
            Console.Write("Input option number: ");
            var input = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            dialogPlayer.SelectOption(optionList[input - 1]);
        }
    }
}
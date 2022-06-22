using System;
using System.Linq;

using Rpg.Client;
using Rpg.Client.Assets;
using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Speech;

namespace DialoguePlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventCatalog = new StaticTextEventCatalog();
            eventCatalog.Init();

            var event1 = eventCatalog.Events.First(x => x.BeforeCombatStartNodeSid != null);

            var dialogue = eventCatalog.GetDialogue(event1.BeforeCombatStartNodeSid);

            var unitSchemeCatalog = new UnitSchemeCatalog(new BalanceTable());
            var storyPointInitializer = new StoryPointCatalog();
            var dice = new LinearDice();
            var globeProvider = new GlobeProvider(dice, unitSchemeCatalog,
                new BiomeGenerator(dice, unitSchemeCatalog, eventCatalog), eventCatalog, storyPointInitializer);
            globeProvider.GenerateNew();
            var globe = globeProvider.Globe;

            var dialogPlayer = new Rpg.Client.Core.Dialogues.DialoguePlayer(dialogue, new DialogueContextFactory(globe, new StoryPointCatalog()));

            while (!dialogPlayer.IsEnd)
            {
                foreach (var textFragment in dialogPlayer.CurrentTextFragments)
                {
                    var speakerLocalized = GameObjectHelper.GetLocalized(textFragment.Speaker);
                    var localizedSpeach = DialogueResources.ResourceManager.GetString(textFragment.TextSid);
                    Console.WriteLine($"{speakerLocalized}: {localizedSpeach}");
                }

                var optionList = dialogPlayer.CurrentOptions.ToArray();
                for (var index = 0; index < optionList.Length; index++)
                {
                    var option = optionList[index];
                    var localizedOptionText = DialogueResources.ResourceManager.GetString(option.TextSid);
                    Console.WriteLine((index + 1) + ". " + localizedOptionText);
                }

                Console.Write("Твой выбор: ");
                var input = int.Parse(Console.ReadLine());

                dialogPlayer.SelectOption(optionList[input - 1]);
            }
        }
    }
}
using System;
using System.Linq;

using Rpg.Client.Assets.DialogueOptionAftermath;
using Rpg.Client.Core;
using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Assets.Catalogs
{
    internal static class EventCatalogHelper
    {
        private const int SPEECH_TEXT_MAX_SYMBOL_COUNT = 60;

        public static EventNode BuildEventNode(
            EventNodeStorageModel nodeStorageModel, EventPosition position,
            string? aftermath,
            IUnitSchemeCatalog unitSchemeCatalog,
            bool splitIntoPages)
        {
            if (!splitIntoPages)
            {
                var fragments = nodeStorageModel.Fragments.Select(CreateEventTextFragment).ToList();

                var optionAftermath = CreateAftermath(aftermath, unitSchemeCatalog);

                var option = new EventOption(position == EventPosition.BeforeCombat ? "Combat" : "Continue",
                    EventNode.EndNode)
                {
                    Aftermath = optionAftermath
                };

                return new EventNode
                {
                    CombatPosition = position,
                    Options = new[]
                    {
                        option
                    },
                    TextBlock = new EventTextBlock
                    {
                        Fragments = fragments
                    }
                };
            }
            else
            {
                var startStorageFragments = nodeStorageModel.Fragments.Take(5).ToArray();
                var innerStorageFragments = nodeStorageModel.Fragments.Skip(5).ToArray();

                var fragments = startStorageFragments.Select(CreateEventTextFragment).ToList();

                var innerFragments = innerStorageFragments.Select(CreateEventTextFragment).ToList();

                var optionAftermath = CreateAftermath(aftermath, unitSchemeCatalog);

                EventOption option;

                if (innerStorageFragments.Any())
                {
                    option = new EventOption("Continue", new EventNode
                    {
                        CombatPosition = position,
                        Options = new[]
                        {
                            new EventOption(position == EventPosition.BeforeCombat ? "Combat" : "Continue",
                                EventNode.EndNode)
                            {
                                Aftermath = optionAftermath
                            }
                        },
                        TextBlock = new EventTextBlock
                        {
                            Fragments = innerFragments
                        }
                    });
                }
                else
                {
                    option = new EventOption(position == EventPosition.BeforeCombat ? "Combat" : "Continue",
                        EventNode.EndNode)
                    {
                        Aftermath = optionAftermath
                    };
                }

                return new EventNode
                {
                    CombatPosition = position,
                    Options = new[]
                    {
                        option
                    },
                    TextBlock = new EventTextBlock
                    {
                        Fragments = fragments
                    }
                };
            }
        }

        private static IOptionAftermath? CreateAftermath(string? aftermath, IUnitSchemeCatalog unitSchemeCatalog)
        {
            IOptionAftermath? optionAftermath = null;
            if (aftermath is not null)
            {
                optionAftermath = aftermath switch
                {
                    "MeetArcher" =>
                        new AddPlayerCharacterOptionAftermath(unitSchemeCatalog.Heroes[UnitName.Archer]),
                    "MeetHerbalist" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Herbalist]),
                    "MeetMonk" =>
                        new AddPlayerCharacterOptionAftermath(unitSchemeCatalog.Heroes[UnitName.Monk]),
                    "MeetSpearman" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Spearman]),
                    "MeetMissionary" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Sage]),
                    "MeetMedjay" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Medjay]),
                    "MeetPriest" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Priest]),
                    "SwordsmanDeepPreying" => new UnitDeepPreyingOptionAftermath(UnitName.Swordsman),
                    _ => optionAftermath
                };
            }

            return optionAftermath;
        }

        private static EventTextFragment CreateEventTextFragment(EventTextFragmentStorageModel fragmentStorageModel)
        {
            var fixedText = StringHelper.FixText(fragmentStorageModel.Text);
            var splitedText = StringHelper.LineBreaking(fixedText, SPEECH_TEXT_MAX_SYMBOL_COUNT);
            return new EventTextFragment
            {
                TextSid = StringHelper.LineBreaking(splitedText, SPEECH_TEXT_MAX_SYMBOL_COUNT),
                Speaker = ParseSpeaker(fragmentStorageModel)
            };
        }

        private static UnitName ParseSpeaker(EventTextFragmentStorageModel fragmentStorageModel)
        {
            if (fragmentStorageModel.Speaker is null)
            {
                return UnitName.Environment;
            }

            var unitName = Enum.Parse<UnitName>(fragmentStorageModel.Speaker);
            return unitName;
        }
    }
}
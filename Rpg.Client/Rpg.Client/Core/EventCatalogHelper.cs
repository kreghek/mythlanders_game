using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Core
{
    internal static class EventCatalogHelper
    {
        public static EventNode BuildEventNode(
            EventNodeStorageModel nodeStorageModel, EventPosition position,
            string? aftermath,
            IUnitSchemeCatalog unitSchemeCatalog,
            bool splitIntoPages)
        {
            if (!splitIntoPages)
            {
                var fragments = new List<EventTextFragment>();
                foreach (var fragmentStorageModel in nodeStorageModel.Fragments)
                {
                    fragments.Add(new EventTextFragment
                    {
                        Text = StringHelper.TempLineBreaking(fragmentStorageModel.Text),
                        Speaker = ParseSpeaker(fragmentStorageModel)
                    });
                }

                var optionAftermath = CreateAftermath(aftermath, unitSchemeCatalog);

                EventOption option;

                option = new EventOption
                {
                    TextSid = position == EventPosition.BeforeCombat ? "Combat" : "Continue",
                    IsEnd = true,
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

                var fragments = new List<EventTextFragment>();
                foreach (var fragmentStorageModel in startStorageFragments)
                {
                    fragments.Add(new EventTextFragment
                    {
                        Text = StringHelper.TempLineBreaking(fragmentStorageModel.Text),
                        Speaker = ParseSpeaker(fragmentStorageModel)
                    });
                }

                var innerFragments = new List<EventTextFragment>();
                foreach (var fragmentStorageModel in innerStorageFragments)
                {
                    innerFragments.Add(new EventTextFragment
                    {
                        Text = StringHelper.TempLineBreaking(fragmentStorageModel.Text),
                        Speaker = ParseSpeaker(fragmentStorageModel)
                    });
                }

                var optionAftermath = CreateAftermath(aftermath, unitSchemeCatalog);

                EventOption option;

                if (innerStorageFragments.Any())
                {
                    option = new EventOption
                    {
                        TextSid = "Continue",
                        IsEnd = false,
                        Next = new EventNode
                        {
                            CombatPosition = position,
                            Options = new[]
                            {
                                new EventOption
                                {
                                    TextSid = position == EventPosition.BeforeCombat ? "Combat" : "Continue",
                                    IsEnd = true,
                                    Aftermath = optionAftermath
                                }
                            },
                            TextBlock = new EventTextBlock
                            {
                                Fragments = innerFragments
                            }
                        }
                    };
                }
                else
                {
                    option = new EventOption
                    {
                        TextSid = position == EventPosition.BeforeCombat ? "Combat" : "Continue",
                        IsEnd = true,
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
                        new AddPlayerCharacterOptionAftermath(unitSchemeCatalog.Heroes[UnitName.Hawk]),
                    "MeetHerbalist" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Rada]),
                    "MeetMonk" =>
                        new AddPlayerCharacterOptionAftermath(unitSchemeCatalog.Heroes[UnitName.Maosin]),
                    "MeetSpearman" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Ping]),
                    "MeetMissionary" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Cheng]),
                    "MeetScorpion" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Amun]),
                    "MeetPriest" => new AddPlayerCharacterOptionAftermath(
                        unitSchemeCatalog.Heroes[UnitName.Kakhotep]),
                    "BerimirDeepPreying" => new UnitDeepPreyingOptionAftermath(UnitName.Berimir),
                    _ => optionAftermath
                };
            }

            return optionAftermath;
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
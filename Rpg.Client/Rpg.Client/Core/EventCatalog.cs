using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text.Json;

using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Core
{
    internal static class EventCatalog
    {
        private static readonly Event[] _events;

        static EventCatalog()
        {
            var rm = new ResourceManager(typeof(PlotResources));
            var serializedPlotString = rm.GetString("MainPlot");


            var testEvents = CreateTestEvents();

            var plotEvents = CreatePlotEvents(serializedPlotString);

            _events = testEvents.Concat(plotEvents).ToArray();
        }

        public static IEnumerable<Event> Events => _events;

        private static IEnumerable<Event> CreatePlotEvents(string? serializedPlotString)
        {
            var eventStorageModelList = JsonSerializer.Deserialize<EventStorageModel[]>(serializedPlotString);

            foreach (var eventStorageModel in eventStorageModelList)
            {
                var locationInfo = GetLocationInfo(eventStorageModel.Name);

                var beforeEventNode = BuildEventNode(eventStorageModel.BeforeCombatNode, EventPosition.BeforeCombat);
                var afterEventNode = BuildEventNode(eventStorageModel.AfterCombatNode, EventPosition.AfterCombat);

                var plotEvent = new Event
                {
                    Biome = locationInfo.Biome,
                    ApplicableOnlyFor = new[] { locationInfo.LocationSid },
                    IsUnique = true,
                    IsHighPriority = true,
                    Sid = eventStorageModel.Name,
                    BeforeCombatStartNode = beforeEventNode,
                    AfterCombatStartNode = afterEventNode
                };

                yield return plotEvent;
            }
        }

        private static EventNode BuildEventNode(EventNodeStorageModel nodeStorageModel, EventPosition position)
        {
            var fragments = new List<EventTextFragment>();

            foreach (var fragmentStrageModel in nodeStorageModel.Fragments)
            {
                fragments.Add(new EventTextFragment
                {
                    TextSid = fragmentStrageModel.Text,
                    Speaker = ParseSpeaker(fragmentStrageModel)
                });
            }

            return new EventNode
            {
                CombatPosition = position,
                Options = new[]
                {
                    new EventOption
                    {
                        TextSid = position == EventPosition.BeforeCombat ? "Combat" : "Continue",
                        IsEnd = true,
                        Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.HerbalistHero)
                    }
                },
                TextBlock = new EventTextBlock
                {
                    Fragments = fragments
                }
            };
        }

        private static UnitName ParseSpeaker(EventTextFragmentStorageModel fragmentStrageModel)
        {
            var unitName = Enum.Parse<UnitName>(fragmentStrageModel.Speaker);
            return unitName;
        }

        private sealed record LocationInfo
        { 
            public BiomeType Biome { get; init; }
            public GlobeNodeSid LocationSid { get; init; }
        }

        private static LocationInfo GetLocationInfo(string name)
        {
            switch (name)
            {
                case "Thicket":
                    return new LocationInfo { Biome = BiomeType.Slavic, LocationSid = GlobeNodeSid.SlavicThicket };

                default:
                    throw new InvalidOperationException();
            }
        }

        private static Event[] CreateTestEvents()
        {
            return Array.Empty<Event>();
            //return new[]
            //            {
            //    CreateTestDialog(1, BiomeType.Slavic),
            //    CreateTestDialog(2, BiomeType.Slavic),
            //    CreateTestDialog(3, BiomeType.Slavic),
            //    CreateTestDialog(4, BiomeType.Slavic),
            //    CreateTestDialog(5, BiomeType.Slavic),
            //    CreateTestDialog(6, BiomeType.Slavic),
            //    CreateTestDialog(7, BiomeType.Slavic),
            //    CreateTestDialog(8, BiomeType.Slavic),
            //    CreateTestDialog(9, BiomeType.Slavic),
            //    CreateTestDialog(10, BiomeType.Slavic),

            //    CreateDependentTestEvent(1, "Тестовое событие 10", BiomeType.Slavic),

            //    CreateMeetHerbalistDialog(),
            //    CreateMeetArcherDialog(),
            //    CreateMeetPriestDialog(),
            //    CreateMeetMissionaryDialog()
            //};
        }

        //private static Event CreateDependentTestEvent(int id, string requiredEventName, BiomeType biomeType)
        //{
        //    var dialogNode1 = new EventNode
        //    {
        //        Text = $"Описание ситуации {id}. Это событие зависит от {requiredEventName}."
        //    };

        //    var dialogNode2 = new EventNode
        //    {
        //        Text = "Описание последствий."
        //    };

        //    dialogNode1.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "Что-то сделать.",
        //            Next = dialogNode2
        //        }
        //    };

        //    dialogNode2.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "В бой!",
        //            IsEnd = true
        //        }
        //    };

        //    var dialog = new Event
        //    {
        //        Name = $"Зависимое Тестовое событие {id}",
        //        Nodes = new[]
        //        {
        //            dialogNode1,
        //            dialogNode2
        //        },
        //        StartNode = dialogNode1,
        //        Biome = biomeType,
        //        RequiredEventsCompleted = new[]
        //        {
        //            requiredEventName
        //        }
        //    };
        //    return dialog;
        //}

        //private static Event CreateMeetArcherDialog()
        //{
        //    var dialogNode1 = new EventNode
        //    {
        //        Text = "Вы встречаете путника. Это лучник."
        //    };

        //    var dialogNode2 = new EventNode
        //    {
        //        Text = "Лучник присоединился к вам."
        //    };

        //    dialogNode1.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "Пригласить в группу.",
        //            Next = dialogNode2,
        //            Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.ArcherHero)
        //        }
        //    };

        //    dialogNode2.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "В бой!",
        //            IsEnd = true
        //        }
        //    };

        //    var dialog = new Event
        //    {
        //        Name = "Ты и я одной крови",
        //        Nodes = new[]
        //        {
        //            dialogNode1,
        //            dialogNode2
        //        },
        //        StartNode = dialogNode1,
        //        IsUnique = true,
        //        SystemMarker = SystemEventMarker.MeetArcher,
        //        Biome = BiomeType.Slavic,
        //        RequiredBiomeLevel = 3
        //    };
        //    return dialog;
        //}

        //private static Event CreateMeetHerbalistDialog()
        //{
        //    var dialogNode1 = new EventNode
        //    {
        //        Text = "Вы встречаете путника. Это травница."
        //    };

        //    var dialogNode2 = new EventNode
        //    {
        //        Text = "Травница присоединилась к вам."
        //    };

        //    dialogNode1.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "Пригласить в группу.",
        //            Next = dialogNode2,
        //            Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.HerbalistHero)
        //        }
        //    };

        //    dialogNode2.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "В бой!",
        //            IsEnd = true
        //        }
        //    };

        //    var dialog = new Event
        //    {
        //        Name = "Собирая гербарий",
        //        Nodes = new[]
        //        {
        //            dialogNode1,
        //            dialogNode2
        //        },
        //        StartNode = dialogNode1,
        //        IsUnique = true,
        //        SystemMarker = SystemEventMarker.MeetHerbalist,
        //        Biome = BiomeType.Slavic,
        //        RequiredBiomeLevel = 6
        //    };
        //    return dialog;
        //}

        //private static Event CreateMeetMissionaryDialog()
        //{
        //    var dialogNode1 = new EventNode
        //    {
        //        Text = "Вы встречаете путника. Это китайский миссионер."
        //    };

        //    var dialogNode2 = new EventNode
        //    {
        //        Text = "Миссионер присоединился к вам."
        //    };

        //    dialogNode1.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "Пригласить в группу.",
        //            Next = dialogNode2,
        //            Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.MissionaryHero)
        //        }
        //    };

        //    dialogNode2.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "В бой!",
        //            IsEnd = true
        //        }
        //    };

        //    var dialog = new Event
        //    {
        //        Name = "Я несу слово",
        //        Nodes = new[]
        //        {
        //            dialogNode1,
        //            dialogNode2
        //        },
        //        StartNode = dialogNode1,
        //        IsUnique = true,
        //        SystemMarker = SystemEventMarker.MeetMissionary,
        //        Biome = BiomeType.China
        //    };
        //    return dialog;
        //}

        //private static Event CreateMeetPriestDialog()
        //{
        //    var dialogNode1 = new EventNode
        //    {
        //        Text = "Вы встречаете путника. Это египетский Жрец."
        //    };

        //    var dialogNode2 = new EventNode
        //    {
        //        Text = "Жрец присоединился к вам."
        //    };

        //    dialogNode1.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "Пригласить в группу.",
        //            Next = dialogNode2,
        //            Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.PriestHero)
        //        }
        //    };

        //    dialogNode2.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "В бой!",
        //            IsEnd = true
        //        }
        //    };

        //    var dialog = new Event
        //    {
        //        Name = "Поклонение песку",
        //        Nodes = new[]
        //        {
        //            dialogNode1,
        //            dialogNode2
        //        },
        //        StartNode = dialogNode1,
        //        IsUnique = true,
        //        SystemMarker = SystemEventMarker.MeetPriest,
        //        Biome = BiomeType.Egypt
        //    };
        //    return dialog;
        //}

        //private static Event CreateTestDialog(int id, BiomeType biomeType)
        //{
        //    var dialogNode1 = new EventNode
        //    {
        //        Text = $"Описание ситуации {id}."
        //    };

        //    var dialogNode2 = new EventNode
        //    {
        //        Text = "Описание последствий."
        //    };

        //    dialogNode1.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "Что-то сделать.",
        //            Next = dialogNode2
        //        }
        //    };

        //    dialogNode2.Options = new[]
        //    {
        //        new EventOption
        //        {
        //            Text = "В бой!",
        //            IsEnd = true
        //        }
        //    };

        //    var dialog = new Event
        //    {
        //        Name = $"Тестовое событие {id}",
        //        Nodes = new[]
        //        {
        //            dialogNode1,
        //            dialogNode2
        //        },
        //        StartNode = dialogNode1,
        //        Biome = biomeType
        //    };
        //    return dialog;
        //}
    }
}
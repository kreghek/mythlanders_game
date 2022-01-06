using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;

using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Core
{
    internal class EventCatalog : IEventCatalog
    {
        private readonly Event[] _events;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public EventCatalog(IUnitSchemeCatalog unitSchemeCatalog)
        {
            _unitSchemeCatalog = unitSchemeCatalog;

            var rm = PlotResources.ResourceManager;
            var serializedPlotString = rm.GetString("MainPlot");

            Debug.Assert(serializedPlotString is not null, "It is required to resources contain serialized plot.");

            var testEvents = CreateTestEvents();

            var plotEvents = CreatePlotEvents(serializedPlotString);

            _events = testEvents.Concat(plotEvents).ToArray();
        }

        private IEnumerable<Event> CreatePlotEvents(string serializedPlotString)
        {
            var eventStorageModelList = JsonSerializer.Deserialize<EventStorageModel[]>(serializedPlotString);

            Debug.Assert(eventStorageModelList is not null, "Plot event required to be correctly serializable.");

            foreach (var eventStorageModel in eventStorageModelList)
            {
                var locationInfo = GetLocationInfo(eventStorageModel.Location);

                var beforeEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.BeforeCombatNode,
                    EventPosition.BeforeCombat,
                    eventStorageModel.Aftermath, _unitSchemeCatalog);
                var afterEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.AfterCombatNode,
                    EventPosition.AfterCombat,
                    aftermath: null, unitSchemeCatalog: _unitSchemeCatalog);

                // System marker used to load saved game. Read it as identifier.
                SystemEventMarker? systemMarker = null;
                if (eventStorageModel.Aftermath is not null)
                {
                    if (Enum.TryParse<SystemEventMarker>(eventStorageModel.Aftermath, out var systemEventMarkerTemp))
                    {
                        systemMarker = systemEventMarkerTemp;
                    }
                }

                var plotEvent = new Event
                {
                    Sid = eventStorageModel.Sid,
                    Biome = locationInfo.Biome,
                    ApplicableOnlyFor = new[] { locationInfo.LocationSid },
                    IsUnique = true,
                    IsHighPriority = true,
                    Title = eventStorageModel.Name,
                    BeforeCombatStartNode = beforeEventNode,
                    AfterCombatStartNode = afterEventNode,
                    SystemMarker = systemMarker,
                    IsGameStart = eventStorageModel == eventStorageModelList.First()
                };

                yield return plotEvent;
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

        private static LocationInfo GetLocationInfo(string location)
        {
            var locationSid = Enum.Parse<GlobeNodeSid>(location);
            var biomeValue = (((int)locationSid) / 100) * 100;
            var biome = (BiomeType)biomeValue;
            return new LocationInfo { Biome = biome, LocationSid = locationSid };
        }

        public IEnumerable<Event> Events => _events;

        private sealed record LocationInfo
        {
            public BiomeType Biome { get; init; }
            public GlobeNodeSid LocationSid { get; init; }
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
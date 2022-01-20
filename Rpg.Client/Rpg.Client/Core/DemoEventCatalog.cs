using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;

using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Core
{
    internal class DemoEventCatalog : IEventCatalog
    {
        private readonly Event[] _events;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public DemoEventCatalog(IUnitSchemeCatalog unitSchemeCatalog)
        {
            _unitSchemeCatalog = unitSchemeCatalog;

            var rm = PlotResources.ResourceManager;
            var serializedPlotString = rm.GetString("MainPlotDemo");

            Debug.Assert(serializedPlotString is not null, "It is required to resources contain serialized plot.");

            var plotEvents = CreatePlotEvents(serializedPlotString ?? string.Empty);

            _events = plotEvents.ToArray();
        }

        private EventNode BuildEventNode(EventNodeStorageModel nodeStorageModel, EventPosition position,
            string? aftermath)
        {
            var fragments = new List<EventTextFragment>();

            foreach (var fragmentStrageModel in nodeStorageModel.Fragments)
            {
                fragments.Add(new EventTextFragment
                {
                    Text = TempLineBreaking(fragmentStrageModel.Text),
                    Speaker = ParseSpeaker(fragmentStrageModel)
                });
            }

            IOptionAftermath? optionAftermath = null;
            if (aftermath is not null)
            {
                optionAftermath = aftermath switch
                {
                    "MeetArcher" =>
                        new AddPlayerCharacterOptionAftermath(_unitSchemeCatalog.PlayerUnits[UnitName.Hawk]),
                    "MeetHerbalist" => new AddPlayerCharacterOptionAftermath(
                        _unitSchemeCatalog.PlayerUnits[UnitName.Rada]),
                    _ => optionAftermath
                };
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
                        Aftermath = optionAftermath
                    }
                },
                TextBlock = new EventTextBlock
                {
                    Fragments = fragments
                }
            };
        }

        private IEnumerable<Event> CreatePlotEvents(string serializedPlotString)
        {
            var eventStorageModelList = JsonSerializer.Deserialize<EventStorageModel[]>(serializedPlotString);

            Debug.Assert(eventStorageModelList is not null, "Plot event required to be correctly serializable.");
            if (eventStorageModelList is null)
            {
                yield break;
            }

            foreach (var eventStorageModel in eventStorageModelList)
            {
                var locationInfo = GetLocationInfo(eventStorageModel.Location);

                var beforeEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.BeforeCombatNode,
                    EventPosition.BeforeCombat,
                    eventStorageModel.BeforeCombatAftermath, _unitSchemeCatalog);
                var afterEventNode = EventCatalogHelper.BuildEventNode(eventStorageModel.AfterCombatNode,
                    EventPosition.AfterCombat,
                    aftermath: null, _unitSchemeCatalog);

                // System marker used to load saved game. Read it as identifier.
                SystemEventMarker? systemMarker = null;
                if (eventStorageModel.BeforeCombatAftermath is not null)
                {
                    if (Enum.TryParse<SystemEventMarker>(eventStorageModel.BeforeCombatAftermath,
                        out var systemEventMarkerTemp))
                    {
                        systemMarker = systemEventMarkerTemp;
                    }
                }

                var plotEvent = new Event
                {
                    Biome = locationInfo.Biome,
                    ApplicableOnlyFor = new[] { locationInfo.LocationSid },
                    IsUnique = true,
                    IsHighPriority = true,
                    Title = eventStorageModel.Name,
                    BeforeCombatStartNode = beforeEventNode,
                    AfterCombatStartNode = afterEventNode,
                    SystemMarker = systemMarker,
                    IsGameStart = eventStorageModel == eventStorageModelList.First(),
                    Sid = eventStorageModel.Sid
                };

                yield return plotEvent;
            }
        }

        private static LocationInfo GetLocationInfo(string location)
        {
            var locationSid = Enum.Parse<GlobeNodeSid>(location);
            var biomeValue = (((int)locationSid) / 100) * 100;
            var biome = (BiomeType)biomeValue;
            return new LocationInfo { Biome = biome, LocationSid = locationSid };
        }

        private static UnitName ParseSpeaker(EventTextFragmentStorageModel fragmentStrageModel)
        {
            if (fragmentStrageModel.Speaker is null)
            {
                return UnitName.Environment;
            }

            var unitName = Enum.Parse<UnitName>(fragmentStrageModel.Speaker);
            return unitName;
        }

        private static string TempLineBreaking(string localizedSpeakerText)
        {
            var items = localizedSpeakerText.Split('\n');
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                if (item.Length > 80)
                {
                    var textRemains = item;
                    do
                    {
                        sb.AppendLine(textRemains.Substring(0, 80));
                        textRemains = textRemains.Remove(0, 80);
                    } while (textRemains.Length > 80);

                    sb.AppendLine(textRemains);
                }
                else
                {
                    sb.AppendLine(item);
                }
            }

            return sb.ToString();
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
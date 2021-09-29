using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal static class EventCatalog
    {
        private static readonly Event[] _events;

        static EventCatalog()
        {
            var testEvents = CreateTestEvents();

            var plotEvents = CreatePlotEvents();

            _events = testEvents.Concat(plotEvents).ToArray();
        }

        public static IEnumerable<Event> Events => _events;

        private static IEnumerable<Event> CreatePlotEvents()
        {
            var slavicPlotEvent1 = new Event
            {
                Biome = BiomeType.Slavic,
                ApplicableOnlyFor = new[] { GlobeNodeSid.SlavicThicket },
                IsUnique = true,
                IsHighPriority = true,
                Sid = "SlavicPlot_1",
                BeforeCombatStartNode = new EventNode
                {
                    CombatPosition = EventPosition.BeforeCombat,
                    TextBlock = new EventTextBlock
                    {
                        Fragments = new[]
                        {
                            new EventTextFragment
                            {
                                Speaker = EventSpeaker.Environment,
                                TextSid = "SlavicPlot_1_b_1"
                            }
                        }
                    },
                    Options = new[]
                    {
                        new EventOption
                        {
                            Text = "В бой!",
                            IsEnd = true
                        }
                    }
                },
                AfterCombatStartNode = new EventNode
                {
                    CombatPosition = EventPosition.AfterCombat,
                    TextBlock = new EventTextBlock
                    {
                        Fragments = new[]
                        {
                            new EventTextFragment
                            {
                                Speaker = EventSpeaker.Berimir,
                                TextSid = "SlavicPlot_1_a_1"
                            },
                            new EventTextFragment
                            {
                                Speaker = EventSpeaker.Environment,
                                TextSid = "SlavicPlot_1_a_2"
                            }
                        }
                    },
                    Options = new[]
                    {
                        new EventOption
                        {
                            Text = "Продолжить",
                            IsEnd = true
                        }
                    }
                }
            };

            yield return slavicPlotEvent1;

            var slavicPlotEvent3 = new Event
            {
                Biome = BiomeType.Slavic,
                ApplicableOnlyFor = new[] { GlobeNodeSid.SlavicSwamp },
                IsUnique = true,
                IsHighPriority = true,
                Sid = "SlavicPlot_3",
                RequiredBiomeLevel = 3,
                RequiredEventsCompleted = new[] { "SlavicPlot_1" },
                BeforeCombatStartNode = new EventNode
                {
                    CombatPosition = EventPosition.BeforeCombat,
                    TextBlock = new EventTextBlock
                    {
                        Fragments = new[]
                        {
                            new EventTextFragment
                            {
                                Speaker = EventSpeaker.Environment,
                                TextSid = "SlavicPlot_3_b_1"
                            }
                        }
                    },
                    Options = new[]
                    {
                        new EventOption
                        {
                            Text = "В бой!",
                            IsEnd = true,
                            Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.ArcherHero)
                        }
                    }
                },
                AfterCombatStartNode = new EventNode
                {
                    CombatPosition = EventPosition.AfterCombat,
                    TextBlock = new EventTextBlock
                    {
                        Fragments = new[]
                        {
                            new EventTextFragment
                            {
                                Speaker = EventSpeaker.Hawk,
                                TextSid = "SlavicPlot_3_a_1"
                            },
                            new EventTextFragment
                            {
                                Speaker = EventSpeaker.Environment,
                                TextSid = "SlavicPlot_3_a_2"
                            }
                        }
                    },
                    Options = new[]
                    {
                        new EventOption
                        {
                            Text = "Продолжить",
                            IsEnd = true
                        }
                    }
                }
            };

            yield return slavicPlotEvent3;
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
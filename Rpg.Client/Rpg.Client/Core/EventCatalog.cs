using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal static class EventCatalog
    {
        private static readonly Event[] _dialogs =
        {
            CreateTestDialog(1, BiomeType.Slavic),
            CreateTestDialog(2, BiomeType.Slavic),
            CreateTestDialog(3, BiomeType.Slavic),
            CreateTestDialog(4, BiomeType.Slavic),
            CreateTestDialog(5, BiomeType.Slavic),
            CreateTestDialog(6, BiomeType.Slavic),
            CreateTestDialog(7, BiomeType.Slavic),
            CreateTestDialog(8, BiomeType.Slavic),
            CreateTestDialog(9, BiomeType.Slavic),
            CreateTestDialog(10, BiomeType.Slavic),

            CreateMeetHerbalistDialog(),
            CreateMeetArcherDialog(),
            CreateMeetPriestDialog(),
            CreateMeetMissionaryDialog()
        };

        public static IEnumerable<Event> Dialogs => _dialogs;

        private static Event CreateMeetHerbalistDialog()
        {
            var dialogNode1 = new EventNode
            {
                Text = "Вы встречаете путника. Это травница."
            };

            var dialogNode2 = new EventNode
            {
                Text = "Травница присоединилась к вам."
            };

            dialogNode1.Options = new[]
            {
                new EventOption
                {
                    Text = "Пригласить в группу.",
                    Next = dialogNode2,
                    Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.HerbalistHero)
                }
            };

            dialogNode2.Options = new[]
            {
                new EventOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Event
            {
                Name = "Собирая гербарий",
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                IsUnique = true,
                SystemMarker = SystemEventMarker.MeetHerbalist,
                Biome = BiomeType.Slavic,
                RequiredBiomeLevel = 10
            };
            return dialog;
        }

        private static Event CreateMeetArcherDialog()
        {
            var dialogNode1 = new EventNode
            {
                Text = "Вы встречаете путника. Это лучник."
            };

            var dialogNode2 = new EventNode
            {
                Text = "Лучник присоединился к вам."
            };

            dialogNode1.Options = new[]
            {
                new EventOption
                {
                    Text = "Пригласить в группу.",
                    Next = dialogNode2,
                    Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.ArcherHero)
                }
            };

            dialogNode2.Options = new[]
            {
                new EventOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Event
            {
                Name = "Ты и я одной крови",
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                IsUnique = true,
                SystemMarker = SystemEventMarker.MeetArcher,
                Biome = BiomeType.Slavic,
                RequiredBiomeLevel = 5
            };
            return dialog;
        }

        private static Event CreateMeetMissionaryDialog()
        {
            var dialogNode1 = new EventNode
            {
                Text = "Вы встречаете путника. Это китайский миссионер."
            };

            var dialogNode2 = new EventNode
            {
                Text = "Миссионер присоединился к вам."
            };

            dialogNode1.Options = new[]
            {
                new EventOption
                {
                    Text = "Пригласить в группу.",
                    Next = dialogNode2,
                    Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.MissionaryHero)
                }
            };

            dialogNode2.Options = new[]
            {
                new EventOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Event
            {
                Name = "Я несу слово",
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                IsUnique = true,
                SystemMarker = SystemEventMarker.MeetMissionary,
                Biome = BiomeType.China
            };
            return dialog;
        }

        private static Event CreateMeetPriestDialog()
        {
            var dialogNode1 = new EventNode
            {
                Text = "Вы встречаете путника. Это египетский Жрец."
            };

            var dialogNode2 = new EventNode
            {
                Text = "Жрец присоединился к вам."
            };

            dialogNode1.Options = new[]
            {
                new EventOption
                {
                    Text = "Пригласить в группу.",
                    Next = dialogNode2,
                    Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.PriestHero)
                }
            };

            dialogNode2.Options = new[]
            {
                new EventOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Event
            {
                Name = "Поклонение песку",
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                IsUnique = true,
                SystemMarker = SystemEventMarker.MeetPriest,
                Biome = BiomeType.Egypt
            };
            return dialog;
        }

        private static Event CreateTestDialog(int id, BiomeType biomeType)
        {
            var dialogNode1 = new EventNode
            {
                Text = $"Описание ситуации {id}."
            };

            var dialogNode2 = new EventNode
            {
                Text = "Описание последствий."
            };

            dialogNode1.Options = new[]
            {
                new EventOption
                {
                    Text = "Что-то сделать.",
                    Next = dialogNode2
                }
            };

            dialogNode2.Options = new[]
            {
                new EventOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Event
            {
                Name = $"Тестовое событие {id}",
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                Biome = biomeType
            };
            return dialog;
        }
    }
}
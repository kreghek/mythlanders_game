using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal static class EventCatalog
    {
        private static readonly Event[] _dialogs =
        {
            CreateTestDialog(1),
            CreateTestDialog(2),
            CreateTestDialog(3),
            CreateTestDialog(4),
            CreateTestDialog(5),
            CreateTestDialog(6),
            CreateTestDialog(7),
            CreateTestDialog(8),
            CreateTestDialog(9),
            CreateTestDialog(10),

            CreateMeetArcherDialog(),
            CreateMeetHerbalistDialog(),
            CreateMeetPriestDialog()
        };

        public static IEnumerable<Event> Dialogs => _dialogs;

        private static Event CreateMeetHerbalistDialog()
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
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                IsUnique = true,
                SystemMarker = SystemEventMarker.MeetArcher
            };
            return dialog;
        }

        private static Event CreateMeetArcherDialog()
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
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                IsUnique = true,
                SystemMarker = SystemEventMarker.MeetHerbalist
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
                Text = "Жрец присоединилась к вам."
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
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1,
                IsUnique = true,
                SystemMarker = SystemEventMarker.MeetPriest
            };
            return dialog;
        }

        private static Event CreateTestDialog(int id)
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
                Nodes = new[]
                {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1
            };
            return dialog;
        }
    }
}
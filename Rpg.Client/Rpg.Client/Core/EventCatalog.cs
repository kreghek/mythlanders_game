using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal static class EventCatalog
    {
        private static readonly Event[] _dialogs =
        {
            CreateTestDialog(),
            CreateNewUnitDialog(),
            CreateNewUnit2Dialog()
        };

        public static IEnumerable<Event> Dialogs => _dialogs;

        private static Event CreateNewUnit2Dialog()
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
                IsUnique = true
            };
            return dialog;
        }

        private static Event CreateNewUnitDialog()
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
                IsUnique = true
            };
            return dialog;
        }

        private static Event CreateTestDialog()
        {
            var dialogNode1 = new EventNode
            {
                Text = "Описание ситуации."
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
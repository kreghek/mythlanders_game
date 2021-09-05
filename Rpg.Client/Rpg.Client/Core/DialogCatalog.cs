using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal static class DialogCatalog
    {
        private static readonly Dialog[] _dialogs =
        {
            CreateTestDialog(),
            CreateNewUnitDialog(),
            CreateNewUnit2Dialog()
        };

        public static IEnumerable<Dialog> Dialogs => _dialogs;

        private static Dialog CreateNewUnitDialog()
        {
            var dialogNode1 = new DialogNode
            {
                Text = "Вы встречаете путника. Это травница."
            };

            var dialogNode2 = new DialogNode
            {
                Text = "Травница присоединилась к вам."
            };

            dialogNode1.Options = new[]
            {
                new DialogOption
                {
                    Text = "Пригласить в группу.",
                    Next = dialogNode2,
                    Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.HerbalistHero)
                }
            };

            dialogNode2.Options = new[]
            {
                new DialogOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Dialog
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

        private static Dialog CreateNewUnit2Dialog()
        {
            var dialogNode1 = new DialogNode
            {
                Text = "Вы встречаете путника. Это лучник."
            };

            var dialogNode2 = new DialogNode
            {
                Text = "Лучник присоединился к вам."
            };

            dialogNode1.Options = new[]
            {
                new DialogOption
                {
                    Text = "Пригласить в группу.",
                    Next = dialogNode2,
                    Aftermath = new AddPlayerCharacterOptionAftermath(UnitSchemeCatalog.ArcherHero)
                }
            };

            dialogNode2.Options = new[]
            {
                new DialogOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Dialog
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

        private static Dialog CreateTestDialog()
        {
            var dialogNode1 = new DialogNode
            {
                Text = "Описание ситуации."
            };

            var dialogNode2 = new DialogNode
            {
                Text = "Описание последствий."
            };

            dialogNode1.Options = new[]
            {
                new DialogOption
                {
                    Text = "Что-то сделать.",
                    Next = dialogNode2
                }
            };

            dialogNode2.Options = new[]
            {
                new DialogOption
                {
                    Text = "В бой!",
                    IsEnd = true
                }
            };

            var dialog = new Dialog
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
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Dialog
    {
        public DialogNode StartNode { get; init; }
        public IEnumerable<DialogNode> Nodes { get; init; }
    }

    internal sealed class DialogNode
    {
        public string Text { get; init; }

        public IEnumerable<DialogOption> Options { get; set; }
    }

    internal sealed class DialogOption
    {
        public string Text { get; init; }
        public DialogNode Next { get; init; }
        public bool IsEnd { get; init; }
    }

    internal static class DialogCatalog
    {
        public static IEnumerable<Dialog> Dialogs => new Dialog[]
            {
                CreateTestDialog()
            };

        private static Dialog CreateTestDialog()
        {

            var dialogNode1 = new DialogNode
            {
                Text = "Описание ситуации.",
            };

            var dialogNode2 = new DialogNode
            {
                Text = "Описание последствий.",
            };

            dialogNode1.Options = new[] {
                    new DialogOption{
                        Text = "Что-то сделать.",
                        Next = dialogNode2
                    }
                };

            dialogNode2.Options = new[] {
                    new DialogOption{
                        Text = "В бой!",
                        IsEnd = true
                    }
                };


            var dialog = new Dialog
            {
                Nodes = new[] {
                    dialogNode1,
                    dialogNode2
                },
                StartNode = dialogNode1
            };
            return dialog;
        }
    }
}

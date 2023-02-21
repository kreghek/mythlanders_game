using System;
using System.Collections.Generic;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class DialogueNode
    {
        public static DialogueNode EndNode { get; } = new(new EventTextBlock(), Array.Empty<DialogueOption>());

        public IReadOnlyCollection<DialogueOption> Options { get; }

        public DialogueNode(EventTextBlock textBlock, IReadOnlyCollection<DialogueOption> options)
        {
            Options = options;
            TextBlock = textBlock;
        }

        public EventTextBlock TextBlock { get; }
    }
}
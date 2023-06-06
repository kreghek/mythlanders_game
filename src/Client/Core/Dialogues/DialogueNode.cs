using System;
using System.Collections.Generic;

namespace Client.Core.Dialogues;

internal sealed class DialogueNode
{
    public DialogueNode(DialogueParagraphContainer textBlock, IReadOnlyCollection<DialogueOption> options)
    {
        Options = options;
        TextBlock = textBlock;
    }

    public static DialogueNode EndNode { get; } = new(new DialogueParagraphContainer(Array.Empty<DialogueParagraph>()),
        Array.Empty<DialogueOption>());

    public IReadOnlyCollection<DialogueOption> Options { get; }

    public DialogueParagraphContainer TextBlock { get; }
}
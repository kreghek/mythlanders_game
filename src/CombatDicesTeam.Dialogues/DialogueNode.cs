using System;
using System.Collections.Generic;

namespace Client.Core.Dialogues;

public sealed class DialogueNode
{
    public DialogueNode(DialogueParagraph textBlock, IReadOnlyCollection<DialogueOption> options)
    {
        Options = options;
        TextBlock = textBlock;
    }

    public static DialogueNode EndNode { get; } = new(new DialogueParagraph(Array.Empty<DialogueSpeech>()),
        Array.Empty<DialogueOption>());

    public IReadOnlyCollection<DialogueOption> Options { get; }

    public DialogueParagraph TextBlock { get; }
}
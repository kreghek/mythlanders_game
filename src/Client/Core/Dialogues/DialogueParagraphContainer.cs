using System.Collections.Generic;

namespace Client.Core.Dialogues;

internal sealed class DialogueParagraphContainer
{
    public DialogueParagraphContainer(IReadOnlyList<DialogueParagraph> paragraphs)
    {
        Paragraphs = paragraphs;
    }

    public IReadOnlyList<DialogueParagraph> Paragraphs { get; }
}
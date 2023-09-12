using System.Collections.Generic;

namespace Client.Core.Dialogues;

public sealed class DialogueParagraph
{
    public DialogueParagraph(IReadOnlyList<DialogueSpeech> paragraphs)
    {
        Paragraphs = paragraphs;
    }

    public IReadOnlyList<DialogueSpeech> Paragraphs { get; }
}
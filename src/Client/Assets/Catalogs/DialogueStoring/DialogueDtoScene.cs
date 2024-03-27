using JetBrains.Annotations;

namespace Client.Assets.Catalogs.DialogueStoring;

internal class DialogueDtoScene
{
    public DialogueDtoOption[]? Options { get; [UsedImplicitly] init; }
    public DialogueDtoParagraph[]? Paragraphs { get; [UsedImplicitly] init; }
}
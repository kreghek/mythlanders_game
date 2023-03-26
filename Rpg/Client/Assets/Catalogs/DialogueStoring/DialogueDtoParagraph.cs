namespace Client.Assets.Catalogs.DialogueStoring;

internal class DialogueDtoParagraph
{
    public DialogueDtoData[]? Env { get; set; }
    public DialogueDtoReaction[]? Reactions { get; set; }
    public string? Speaker { get; set; }
    public string? Text { get; set; }
}
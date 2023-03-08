namespace Client.Assets.Catalogs.DialogueStoring;

class DialogueDtoParagraph
{
    public string? Text { get; set; }
    public string? Speaker { get; set; }
    public DialogueDtoData[]? Env { get; set; }
    public DialogueDtoReaction[]? Reactions { get; set; }
}
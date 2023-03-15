namespace Client.Assets.Catalogs.DialogueStoring;

internal class DialogueDtoOption
{
    public DialogueDtoData[] Aftermaths { get; set; }
    public DialogueDtoData[] Conditions { get; set; }
    public string? Next { get; set; }
    public string Text { get; set; }
}
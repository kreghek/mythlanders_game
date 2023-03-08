namespace Client.Assets.Catalogs.DialogueStoring;

class DialogueDtoOption
{
    public string Text { get; set; }
    public DialogueDtoData[] Aftermaths { get; set; }
    public string? Next { get; set; }
    public DialogueDtoData[] Conditions { get; set; }
}
namespace Client.Assets.Catalogs.DialogueStoring;

internal class DialogueDtoReaction
{
    public string Hero { get; set; }
    public DialogueDtoParagraph[] Paragraphs { get; set; }
    public string Text { get; set; }
}
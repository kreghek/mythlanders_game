namespace LeDialoduesEditorResGenerator.Serialization;

class DialogueDtoReaction
{
    public string Hero { get; set; }
    public string Text { get; set; }
    public DialogueDtoParagraph[] Paragraphs { get; set; }
}
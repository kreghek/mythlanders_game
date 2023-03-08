namespace LeDialoduesEditorResGenerator.Serialization;

class DialogueDtoParagraph
{
    public string Text { get; set; }
    public string Speaker { get; set; }
    public DialogueDtoData[] Env { get; set; }
    public DialogueDtoReaction[] Reactions { get; set; }
}
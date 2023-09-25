namespace CombatDicesTeam.Dialogues;

public sealed class DialoguePlayer<TParagraphConditionContext, TAftermathContext>
{
    private readonly IDialogueContextFactory<TParagraphConditionContext, TAftermathContext> _contextFactory;
    private DialogueNode<TParagraphConditionContext, TAftermathContext> _currentNode;

    public DialoguePlayer(Dialogue<TParagraphConditionContext, TAftermathContext> dialogue,
        IDialogueContextFactory<TParagraphConditionContext, TAftermathContext> contextFactory)
    {
        _currentNode = dialogue.Root;
        _contextFactory = contextFactory;

        var conditionContext = _contextFactory.CreateParagraphConditionContext();
        CurrentTextFragments = GetTextBlockParagraphs(conditionContext);
        CurrentOptions = _currentNode.Options.ToArray();
    }

    public IReadOnlyCollection<DialogueOption<TParagraphConditionContext, TAftermathContext>> CurrentOptions
    {
        get;
        private set;
    }

    public IReadOnlyList<DialogueSpeech<TParagraphConditionContext, TAftermathContext>> CurrentTextFragments
    {
        get;
        private set;
    }

    public bool IsEnd => _currentNode == DialogueNode<TParagraphConditionContext, TAftermathContext>.EndNode;

    public void SelectOption(DialogueOption<TParagraphConditionContext, TAftermathContext> option)
    {
        var context = _contextFactory.CreateParagraphConditionContext();

        _currentNode = option.Next;

        if (_currentNode != DialogueNode<TParagraphConditionContext, TAftermathContext>.EndNode)
        {
            CurrentTextFragments = GetTextBlockParagraphs(context);
            CurrentOptions = _currentNode.Options.ToArray();
        }
        else
        {
            CurrentTextFragments = ArraySegment<DialogueSpeech<TParagraphConditionContext, TAftermathContext>>.Empty;
            CurrentOptions = ArraySegment<DialogueOption<TParagraphConditionContext, TAftermathContext>>.Empty;
        }

        option.Aftermath?.Apply(_contextFactory.CreateAftermathContext());
    }

    private IReadOnlyList<DialogueSpeech<TParagraphConditionContext, TAftermathContext>> GetTextBlockParagraphs(
        TParagraphConditionContext dialogueParagraphConditionContext)
    {
        var paragraphs = _currentNode.TextBlock.Paragraphs
            .Where(x => x.Conditions.All(c => c.Check(dialogueParagraphConditionContext))).ToArray();

        return paragraphs;
    }
}
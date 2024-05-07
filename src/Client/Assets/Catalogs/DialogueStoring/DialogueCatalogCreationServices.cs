namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed record DialogueCatalogCreationServices<TParagraphConditionContext, TAftermathContext>(
    IDialogueParagraphEffectCreator<TAftermathContext> ParagraphEffectCreator,
    IDialogueOptionAftermathCreator<TAftermathContext> OptionAftermathCreator,
    IDialogueConditionCreator<TParagraphConditionContext> ParagraphConditionCreator);
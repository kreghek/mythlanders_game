namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed record DialogueCatalogCreationServices<TAftermathContext>(
    IDialogueParagraphEffectCreator<TAftermathContext> ParagraphEffectCreator,
    IDialogueOptionAftermathCreator<TAftermathContext> OptionAftermathCreator);
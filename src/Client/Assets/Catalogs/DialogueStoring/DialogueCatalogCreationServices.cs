namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed record DialogueCatalogCreationServices<TAftermathContext>(
    IDialogueEnvironmentEffectCreator<TAftermathContext> ParagraphEffectCreator,
    IDialogueOptionAftermathCreator<TAftermathContext> OptionAftermathCreator);
namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed record DialogueCatalogCreationServices<TAftermathContext>(
    IDialogueEnvironmentEffectCreator<TAftermathContext> EnvEffectCreator,
    IDialogueOptionAftermathCreator<TAftermathContext> OptionAftermathCreator);
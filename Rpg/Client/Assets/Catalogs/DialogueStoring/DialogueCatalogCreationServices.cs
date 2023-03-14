using Rpg.Client.Assets.Catalogs;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed record DialogueCatalogCreationServices(IDialogueEnvironmentEffectCreator EnvEffectCreator,
    IDialogueOptionAftermathCreator OptionAftermathCreator);
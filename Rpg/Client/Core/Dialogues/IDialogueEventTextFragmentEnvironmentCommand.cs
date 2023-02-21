namespace Client.Core.Dialogues;

internal interface IDialogueEventTextFragmentEnvironmentCommand
{
    void Execute(IDialogueTextEventSoundManager soundEffectManager);
}
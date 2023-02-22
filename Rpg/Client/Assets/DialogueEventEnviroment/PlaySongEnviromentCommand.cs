using Client.Core.Dialogues;

namespace Client.Assets.DialogueEventEnviroment;

internal class PlaySongEnviromentCommand : IDialogueEventTextFragmentEnvironmentCommand
{
    private readonly string _resourceName;

    public PlaySongEnviromentCommand(string resourceName)
    {
        _resourceName = resourceName;
    }

    public void Execute(IDialogueTextEventSoundManager soundEffectManager)
    {
        soundEffectManager.PlaySong(_resourceName);
    }
}
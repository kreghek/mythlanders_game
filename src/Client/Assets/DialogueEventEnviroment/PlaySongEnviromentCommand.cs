using Client.Core.Dialogues;

namespace Client.Assets.DialogueEventEnviroment;

internal sealed class PlaySongEnviromentCommand : IDialogueEnvironmentEffect
{
    private readonly string _resourceName;

    public PlaySongEnviromentCommand(string resourceName)
    {
        _resourceName = resourceName;
    }

    public void Execute(IDialogueEnvironmentManager soundEffectManager)
    {
        soundEffectManager.PlaySong(_resourceName);
    }
}
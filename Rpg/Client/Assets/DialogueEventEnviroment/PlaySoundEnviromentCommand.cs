using Client.Core.Dialogues;

namespace Client.Assets.DialogueEventEnviroment;

internal class PlaySoundEnviromentCommand : IDialogueEventTextFragmentEnvironmentCommand
{
    private readonly string _effectSid;
    private readonly string _resourceName;

    public PlaySoundEnviromentCommand(string effectSid, string resourceName)
    {
        _effectSid = effectSid;
        _resourceName = resourceName;
    }

    public void Execute(IDialogueTextEventSoundManager soundEffectManager)
    {
        soundEffectManager.PlayEffect(_effectSid, _resourceName);
    }
}
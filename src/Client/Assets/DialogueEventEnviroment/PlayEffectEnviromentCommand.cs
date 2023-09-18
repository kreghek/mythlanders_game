using Client.Core.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueEventEnviroment;

internal sealed class PlayEffectEnviromentCommand : IDialogueEnvironmentEffect
{
    private readonly string _effectSid;
    private readonly string _resourceName;

    public PlayEffectEnviromentCommand(string effectSid, string resourceName)
    {
        _effectSid = effectSid;
        _resourceName = resourceName;
    }

    public void Execute(IDialogueEnvironmentManager soundEffectManager)
    {
        soundEffectManager.PlayEffect(_effectSid, _resourceName);
    }
}